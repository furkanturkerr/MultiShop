using AutoMapper;
using MongoDB.Driver;
using MultiShop.Catalog.Dtos.ProductImageDtos;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.ProductImageServices;

public class ProductImageService : IProductImageService
{
    private readonly IMongoCollection<ProductImage> _collection;
    private readonly IMapper _mapper;

    public ProductImageService(IMapper mapper, IDatabaseSettings databaseSettings)
    {
        var  client = new MongoClient(databaseSettings.ConnectionString);
        var database = client.GetDatabase(databaseSettings.DatabaseName);
        _collection = database.GetCollection<ProductImage>(databaseSettings.ProductImageCollectionName);
        _mapper = mapper;
    }

    public async Task<List<ResultProductImageDto>> GetAllCategoriesAsync()
    {
        var values = await _collection.FindAsync(x=> true);
        return  _mapper.Map<List<ResultProductImageDto>>(values);
    }

    public async Task CreateProductImageAsync(CreateProductImageDto dto)
    {
        var values = _mapper.Map<ProductImage>(dto);
        await _collection.InsertOneAsync(values);
    }

    public async Task UpdateProductImageAsync(UpdateProductImageDto dto)
    {
        var values = _mapper.Map<ProductImage>(dto);
        await _collection.FindOneAndReplaceAsync(x=>x.ProductImageId == dto.ProductImageId, values);
    }

    public async Task<GetByIdProductImageDto> GetProductImageByIdAsync(string id)
    {
        var value = await _collection.FindAsync(x=>x.ProductImageId == id);
        return _mapper.Map<GetByIdProductImageDto>(value);
    }

    public async Task DeleteProductImageByIdAsync(string id)
    {
        await _collection.DeleteOneAsync(x=>x.ProductImageId == id);
    }
}