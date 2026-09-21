using AutoMapper;
using MongoDB.Driver;
using MultiShop.Catalog.Dtos.FeaturedDtos;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.FeaturedServices;

public class FeaturedService : IFeaturedService
{
    private readonly IMongoCollection<Featured> _collection;
    private readonly IMapper _mapper;

    public FeaturedService(IMapper mapper, IDatabaseSettings databaseSettings)
    {
        var client = new MongoClient(databaseSettings.ConnectionString);
        var database = client.GetDatabase(databaseSettings.DatabaseName);
        _collection = database.GetCollection<Featured>(databaseSettings.FeaturedCollectionName);
        _mapper = mapper;
    }

    public async Task<List<ResultFeaturedDto>> GetAllFeaturedAsync()
    {
        var values = await  _collection.Find(x => true).ToListAsync();
        return _mapper.Map<List<ResultFeaturedDto>>(values);
    }

    public async Task CreateFeaturedAsync(CreateFeaturedDto dto)
    {
        var value = _mapper.Map<Featured>(dto);
        await _collection.InsertOneAsync(value);
    }

    public async Task UpdateFeaturedAsync(UpdateFeaturedDto dto)
    {
        var value = _mapper.Map<Featured>(dto);
        await _collection.ReplaceOneAsync(x => x.FeaturedId == dto.FeaturedId, value);
    }

    public async Task<UpdateFeaturedDto> GetFeaturedByIdAsync(string id)
    {
        var value = await _collection.Find(x=>x.FeaturedId == id).FirstOrDefaultAsync();
        return _mapper.Map<UpdateFeaturedDto>(value);
    }

    public async Task DeleteFeaturedByIdAsync(string id)
    {
        await _collection.DeleteOneAsync(x=>x.FeaturedId == id);
    }

    public async Task<List<ResultFeaturedDto>> GetFeaturedByStatusAsync()
    {
        var values = await _collection.Find(x => true & x.IsActive == true).ToListAsync();
        return _mapper.Map<List<ResultFeaturedDto>>(values);
    }
}