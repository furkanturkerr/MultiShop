using AutoMapper;
using MongoDB.Driver;
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

    public async Task<List<ResultProductDto>> GetAllCategoriesAsync()
    {
        var values = await _collection.Find(x => true).ToListAsync();
        return  _mapper.Map<List<ResultProductDto>>(values);
    }

    public async Task CreateProductAsync(CreateProductDto dto)
    {
        var values = _mapper.Map<Product>(dto);
        await _collection.InsertOneAsync(values);
    }

    public async Task UpdateProductAsync(UpdateProductDto dto)
    {
        var update = Builders<Product>.Update
            .Set(x => x.ProductName, dto.ProductName)
            .Set(x => x.ProductDescription, dto.ProductDescription)
            .Set(x => x.ProductPrice, dto.ProductPrice)
            .Set(x => x.ProductImageUrl, dto.ProductImageUrl)
            .Set(x => x.CategoryId, dto.CategoryId);
        await _collection.UpdateOneAsync(x => x.ProductId == dto.ProductId, update);
    }

    public async Task<GetByIdProductDto> GetProductByIdAsync(string id)
    {
        var value = await _collection.Find(x => x.ProductId == id).FirstOrDefaultAsync();
        return _mapper.Map<GetByIdProductDto>(value);
    }

    public async Task DeleteProductByIdAsync(string id)
    {
        await _collection.DeleteOneAsync(x=>x.ProductId == id);
    }

    public async Task<List<ResultProductWithCategory>> GetAllProductsWithCategoryAsync()
    {
        var products = await _collection
            .Find(x => true)
            .ToListAsync();

        var categories = await _categoryCollection
            .Find(x => true)
            .ToListAsync();

        foreach (var product in products)
        {
            product.Category = categories.FirstOrDefault(
                x => x.CategoryId == product.CategoryId
            );
        }

        return _mapper.Map<List<ResultProductWithCategory>>(products);
    }
}
