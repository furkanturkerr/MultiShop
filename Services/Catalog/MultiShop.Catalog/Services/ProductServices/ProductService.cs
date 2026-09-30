using AutoMapper;
using System.Text.RegularExpressions;
using MongoDB.Bson;
using MongoDB.Driver;
using MultiShop.Catalog.Dtos.CategoryDtos;
using MultiShop.Catalog.Dtos.ProductDtos;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.ProductServices;

public class ProductService : IProductService
{
    private readonly IMongoCollection<Product> _collection;
    private readonly IMapper _mapper;
    private readonly IMongoCollection<Category> _categoryCollection;

    public ProductService(IMapper mapper, IDatabaseSettings databaseSettings)
    {
        var  client = new MongoClient(databaseSettings.ConnectionString);
        var database = client.GetDatabase(databaseSettings.DatabaseName);
        _collection = database.GetCollection<Product>(databaseSettings.ProductCollectionName);
        _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
        _mapper = mapper;
    }

    public async Task<ProductListResultDto> GetFilteredProductsAsync(ProductFilterDto filters, CancellationToken cancellationToken = default)
    {
        filters.Q = filters.Q?.Trim();
        filters.CategoryId = filters.CategoryId?.Trim().ToLowerInvariant();
        filters.PageSize = Math.Clamp(filters.PageSize, 1, 48);
        if (filters.MinPrice < 0) filters.MinPrice = 0;
        if (filters.MaxPrice < 0) filters.MaxPrice = 0;
        if (filters.MinPrice.HasValue && filters.MaxPrice.HasValue && filters.MinPrice > filters.MaxPrice)
            (filters.MinPrice, filters.MaxPrice) = (filters.MaxPrice, filters.MinPrice);

        var builder = Builders<Product>.Filter;
        var filter = builder.Empty;
        if (!string.IsNullOrWhiteSpace(filters.CategoryId))
            filter &= builder.Eq(product => product.CategoryId, filters.CategoryId);
        if (!string.IsNullOrWhiteSpace(filters.Q))
            filter &= builder.Regex(product => product.ProductName, new BsonRegularExpression(Regex.Escape(filters.Q), "i"));
        if (filters.MinPrice.HasValue)
            filter &= builder.Gte(product => product.ProductPrice, filters.MinPrice.Value);
        if (filters.MaxPrice.HasValue)
            filter &= builder.Lte(product => product.ProductPrice, filters.MaxPrice.Value);

        filters.Sort = filters.Sort is "price-asc" or "price-desc" or "name-desc" ? filters.Sort : "name-asc";
        var sortBuilder = Builders<Product>.Sort;
        var sort = filters.Sort switch
        {
            "price-asc" => sortBuilder.Ascending(product => product.ProductPrice),
            "price-desc" => sortBuilder.Descending(product => product.ProductPrice),
            "name-desc" => sortBuilder.Descending(product => product.ProductName),
            _ => sortBuilder.Ascending(product => product.ProductName)
        };
        sort = sort.Ascending(product => product.ProductId);

        var totalCount = checked((int)await _collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken));
        var totalPages = (int)Math.Ceiling(totalCount / (double)filters.PageSize);
        filters.Page = Math.Clamp(filters.Page, 1, Math.Max(1, totalPages));

        // Filtering, sorting and pagination run in MongoDB before products are loaded.
        var products = await _collection
            .Find(filter, new FindOptions { Collation = new Collation("en", strength: CollationStrength.Secondary) })
            .Sort(sort)
            .Skip((filters.Page - 1) * filters.PageSize)
            .Limit(filters.PageSize)
            .ToListAsync(cancellationToken);

        var categories = await _categoryCollection.Find(Builders<Category>.Filter.Empty).ToListAsync(cancellationToken);
        var categoryLookup = categories.ToDictionary(category => category.CategoryId);
        foreach (var product in products)
            product.Category = categoryLookup.GetValueOrDefault(product.CategoryId)!;

        // MongoDB returns category counts, rather than the entire product collection.
        var counts = await _collection.Aggregate()
            .Group(new BsonDocument
            {
                { "_id", "$CategoryId" },
                { "count", new BsonDocument("$sum", 1) }
            })
            .ToListAsync(cancellationToken);

        return new ProductListResultDto
        {
            Filters = filters,
            Products = _mapper.Map<List<ResultProductWithCategory>>(products),
            Categories = _mapper.Map<List<ResultCategoryDto>>(categories),
            CategoryCounts = counts.Where(count => !count["_id"].IsBsonNull)
                .ToDictionary(count => count["_id"].ToString()!, count => count["count"].ToInt32()),
            CatalogCount = counts.Sum(count => count["count"].ToInt32()),
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }

    public async Task<List<ResultProductDto>> GetAllCategoriesAsync()
    {
        var values = await _collection.Find(x => true).ToListAsync();
        return  _mapper.Map<List<ResultProductDto>>(values);
    }

    public async Task<bool> CreateProductAsync(CreateProductDto dto)
    {
        var options = await PrepareOptionsAsync(dto.CategoryId, dto.Options);
        if (options is null)
            return false;

        var product = _mapper.Map<Product>(dto);
        product.Options = options;
        await _collection.InsertOneAsync(product);
        return true;
    }

    public async Task<bool> UpdateProductAsync(UpdateProductDto dto)
    {
        if (!ObjectId.TryParse(dto.ProductId, out _))
            return false;
        var options = await PrepareOptionsAsync(dto.CategoryId, dto.Options);
        if (options is null)
            return false;

        var update = Builders<Product>.Update
            .Set(x => x.ProductName, dto.ProductName)
            .Set(x => x.ProductDescription, dto.ProductDescription)
            .Set(x => x.ProductPrice, dto.ProductPrice)
            .Set(x => x.ProductImageUrl, dto.ProductImageUrl)
            .Set(x => x.CategoryId, dto.CategoryId)
            .Set(x => x.Options, options);
        var result = await _collection.UpdateOneAsync(x => x.ProductId == dto.ProductId, update);
        return result.MatchedCount > 0;
    }

    public async Task<GetByIdProductDto?> GetProductByIdAsync(string id)
    {
        var product = await _collection.Find(x => x.ProductId == id).FirstOrDefaultAsync();
        if (product is null)
            return null;

        var category = await _categoryCollection.Find(x => x.CategoryId == product.CategoryId).FirstOrDefaultAsync();
        var names = category?.OptionNames ?? new List<string>();
        product.Options = names.Select(name => product.Options.FirstOrDefault(x =>
                string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase)))
            .Where(option => option is not null && option.Values.Count > 0)
            .Select(option => option!).ToList();
        return _mapper.Map<GetByIdProductDto>(product);
    }

    public async Task DeleteProductByIdAsync(string id)
    {
        await _collection.DeleteOneAsync(x => x.ProductId == id);
    }

    public async Task<List<ResultProductWithCategory>> GetAllProductsWithCategoryAsync()
    {
        var products = await _collection.Find(x => true).ToListAsync();

        var categories = await _categoryCollection.Find(x => true).ToListAsync();

        foreach (var product in products)
        {
            product.Category = categories.FirstOrDefault(x => x.CategoryId == product.CategoryId);
        }

        return _mapper.Map<List<ResultProductWithCategory>>(products);
    }

    public async Task<List<ResultProductWithCategory>> GetProductsByCategoryIdAsync(string categoryId)
    {
        var products = await _collection.Find(x => x.CategoryId == categoryId).ToListAsync();
        var category = await _categoryCollection.Find(x => x.CategoryId == categoryId).FirstOrDefaultAsync();
        foreach (var product in products)
            product.Category = category!;
        return _mapper.Map<List<ResultProductWithCategory>>(products);
    }

    private async Task<List<ProductOption>?> PrepareOptionsAsync(string categoryId, List<ProductOptionDto> submittedOptions)
    {
        if (!ObjectId.TryParse(categoryId, out _) || submittedOptions is null)
            return null;

        var category = await _categoryCollection.Find(x => x.CategoryId == categoryId).FirstOrDefaultAsync();
        if (category is null)
            return null;

        var options = new List<ProductOption>();
        var usedNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var submitted in submittedOptions)
        {
            if (submitted is null || string.IsNullOrWhiteSpace(submitted.Name) || submitted.Values is null)
                return null;
            var name = category.OptionNames.FirstOrDefault(x =>
                string.Equals(x, submitted.Name.Trim(), StringComparison.OrdinalIgnoreCase));
            if (name is null || !usedNames.Add(name))
                return null;

            var values = submitted.Values.Select(x => x.Trim()).ToList();
            if (values.Count == 0)
                continue;
            if (values.Distinct(StringComparer.OrdinalIgnoreCase).Count() != values.Count)
                return null;

            options.Add(new ProductOption { Name = name, Values = values });
        }
        return options;
    }
}
