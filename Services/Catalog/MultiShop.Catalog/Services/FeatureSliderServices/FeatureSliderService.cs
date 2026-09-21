using AutoMapper;
using MongoDB.Driver;
using MultiShop.Catalog.Dtos.FeatureSldierDtos;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.FeatureSliderServices;

public class FeatureSliderService : IFeatureSliderService
{
    private readonly IMongoCollection<FeatureSlider> _collection;
    private readonly IMapper _mapper;

    public FeatureSliderService(IMapper mapper, IDatabaseSettings databaseSettings)
    {
        var client = new MongoClient(databaseSettings.ConnectionString);
        var database = client.GetDatabase(databaseSettings.DatabaseName);
        _collection = database.GetCollection<FeatureSlider>(databaseSettings.FeatureSliderCollectionName);
        _mapper = mapper;
    }

    public async Task<List<ResultFeatureSliderDto>> GetAllFeatureSliderAsync()
    {
        var values = await  _collection.Find(x => true).ToListAsync();
        return _mapper.Map<List<ResultFeatureSliderDto>>(values);
    }
    
    public async Task CreateFeatureSliderAsync(CreateFeatureSliderDto dto)
    {
        var value = _mapper.Map<FeatureSlider>(dto);
        await _collection.InsertOneAsync(value);
    }

    public async Task UpdateFeatureSliderAsync(UpdateFeatureSliderDto dto)
    {
        var value = _mapper.Map<FeatureSlider>(dto);
        await _collection.ReplaceOneAsync(x => x.FeatureSliderId == dto.FeatureSliderId, value);
    }

    public async Task<UpdateFeatureSliderDto> GetFeatureSliderByIdAsync(string id)
    {
        var value = await _collection.Find(x=>x.FeatureSliderId == id).FirstOrDefaultAsync();
        return _mapper.Map<UpdateFeatureSliderDto>(value);
    }

    public async Task DeleteFeatureSliderByIdAsync(string id)
    {
        await _collection.DeleteOneAsync(x=>x.FeatureSliderId == id);
    }

    public async Task<List<ResultFeatureSliderDto>> GetSliderByStatusAsync()
    {
        var values = await _collection.Find(x => true & x.IsActive == true).ToListAsync();
        return _mapper.Map<List<ResultFeatureSliderDto>>(values);
    }
}