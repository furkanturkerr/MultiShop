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
        var values = await _collection.Find(x => true).ToListAsync();
        return  _mapper.Map<List<ResultProductImageDto>>(values);
    }

    public async Task CreateProductImageAsync(CreateProductImageDto dto)
    {
        var values = _mapper.Map<ProductImage>(dto);
        await _collection.InsertOneAsync(values);
    }

    public async Task UpdateProductImageAsync(UpdateProductImageDto dto)
    {
        var update = Builders<ProductImage>.Update
            .Set(x => x.ImageUrl1, dto.ImageUrl1)
            .Set(x => x.ImageUrl2, dto.ImageUrl2)
            .Set(x => x.ImageUrl3, dto.ImageUrl3)
            .Set(x => x.ProductId, dto.ProductId)
            .Set(x => x.ImageUrl4, dto.ImageUrl4);
        await _collection.UpdateOneAsync(x => x.ProductImageId == dto.ProductImageId, update);
    }

    public async Task<GetByIdProductImageDto> GetProductImageByIdAsync(string id)
    {
        var value = await _collection.Find(x => x.ProductImageId == id).FirstOrDefaultAsync();
        return _mapper.Map<GetByIdProductImageDto>(value);
    }

    public async Task DeleteProductImageByIdAsync(string id)
    {
        await _collection.DeleteOneAsync(x=>x.ProductImageId == id);
    }

    public async Task<GetByIdProductImageDto> GetProductImageByProductIdAsync(string productId)
    {
        var value = await _collection.Find(x => x.ProductId == productId).FirstOrDefaultAsync();
        return _mapper.Map<GetByIdProductImageDto>(value);
    }
}
