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

    public ProductService(IMapper mapper, IDatabaseSettings databaseSettings)
    {
        var  client = new MongoClient(databaseSettings.ConnectionString);
        var database = client.GetDatabase(databaseSettings.DatabaseName);
        _collection = database.GetCollection<Product>(databaseSettings.ProductCollectionName);
        _mapper = mapper;
    }

    public async Task<List<ResultProductDto>> GetAllCategoriesAsync()
    {
        var values = await _collection.FindAsync(x=> true);
        return  _mapper.Map<List<ResultProductDto>>(values);
    }

    public async Task CreateProductAsync(CreateProductDto dto)
    {
        var values = _mapper.Map<Product>(dto);
        await _collection.InsertOneAsync(values);
    }

    public async Task UpdateProductAsync(UpdateProductDto dto)
    {
        var values = _mapper.Map<Product>(dto);
        await _collection.FindOneAndReplaceAsync(x=>x.ProductId == dto.ProductId, values);
    }

    public async Task<GetByIdProductDto> GetProductByIdAsync(string id)
    {
        var value = await _collection.FindAsync(x=>x.ProductId == id);
        return _mapper.Map<GetByIdProductDto>(value);
    }

    public async Task DeleteProductByIdAsync(string id)
    {
        await _collection.DeleteOneAsync(x=>x.ProductId == id);
    }
}