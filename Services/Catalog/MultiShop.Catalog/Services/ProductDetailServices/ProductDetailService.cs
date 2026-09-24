using AutoMapper;
using MongoDB.Driver;
using MultiShop.Catalog.Dtos.ProductDetailDtos;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.ProductDetailServices;

public class ProductDetailService : IProductDetailService
{
    private readonly IMongoCollection<ProductDetail> _collection;
    private readonly IMapper _mapper;

    public ProductDetailService(IMapper mapper, IDatabaseSettings databaseSettings)
    {
        var  client = new MongoClient(databaseSettings.ConnectionString);
        var database = client.GetDatabase(databaseSettings.DatabaseName);
        _collection = database.GetCollection<ProductDetail>(databaseSettings.ProductDetailCollectionName);
        _mapper = mapper;
    }

    public async Task<List<ResultProductDetailDto>> GetAllCategoriesAsync()
    {
        var values = await _collection.Find(x => true).ToListAsync();
        return  _mapper.Map<List<ResultProductDetailDto>>(values);
    }

    public async Task CreateProductDetailAsync(CreateProductDetailDto dto)
    {
        var values = _mapper.Map<ProductDetail>(dto);
        await _collection.InsertOneAsync(values);
    }

    public async Task UpdateProductDetailAsync(UpdateProductDetailDto dto)
    {
        var update = Builders<ProductDetail>.Update
            .Set(x => x.ProductDescription, dto.ProductDescription)
            .Set(x => x.ProductInfo, dto.ProductInfo)
            .Set(x => x.ProductId, dto.ProductId);
        await _collection.UpdateOneAsync(x => x.ProductDetailId == dto.ProductDetailId, update);
    }

    public async Task<GetByIdProductDetailDto> GetProductDetailByIdAsync(string id)
    {
        var value = await _collection.Find(x => x.ProductDetailId == id).FirstOrDefaultAsync();
        return _mapper.Map<GetByIdProductDetailDto>(value);
    }

    public async Task DeleteProductDetailByIdAsync(string id)
    {
        await _collection.DeleteOneAsync(x=>x.ProductDetailId == id);
    }

    public async Task<GetByIdProductDetailDto> GetProductDetailByProductIdAsync(string productId)
    {
        var value = await _collection.Find(x=>x.ProductId == productId).FirstOrDefaultAsync();
        return _mapper.Map<GetByIdProductDetailDto>(value);
    }
}
