using AutoMapper;
using MongoDB.Driver;
using MultiShop.Catalog.Dtos.BrandDtos;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.BrandServices;

public class BrandService : IBrandService
{
    private readonly IMongoCollection<Brand> _collection;
    private readonly IMapper _mapper;

    public BrandService(IMapper mapper, IDatabaseSettings databaseSettings)
    {
        var client = new MongoClient(databaseSettings.ConnectionString);
        var database = client.GetDatabase(databaseSettings.DatabaseName);
        _collection = database.GetCollection<Brand>(databaseSettings.BrandCollectionName);
        _mapper = mapper;
    }

    public async Task<List<ResultBrandDto>> GetAllBrandAsync()
    {
        var values = await  _collection.Find(x => true).ToListAsync();
        return _mapper.Map<List<ResultBrandDto>>(values);
    }

    public async Task CreateBrandAsync(CreateBrandDto dto)
    {
        var value = _mapper.Map<Brand>(dto);
        await _collection.InsertOneAsync(value);
    }

    public async Task UpdateBrandAsync(UpdateBrandDto dto)
    {
        var value = _mapper.Map<Brand>(dto);
        await _collection.ReplaceOneAsync(x => x.BrandId == dto.BrandId, value);
    }

    public async Task<UpdateBrandDto> GetBrandByIdAsync(string id)
    {
        var value = await _collection.Find(x=>x.BrandId == id).FirstOrDefaultAsync();
        return _mapper.Map<UpdateBrandDto>(value);
    }

    public async Task DeleteBrandByIdAsync(string id)
    {
        await _collection.DeleteOneAsync(x=>x.BrandId == id);
    }

    public async Task<List<ResultBrandDto>> GetBrandByStatusAsync()
    {
        var value = await _collection.Find(x=>true & x.IsActive).ToListAsync();
        return _mapper.Map<List<ResultBrandDto>>(value);
    }
}