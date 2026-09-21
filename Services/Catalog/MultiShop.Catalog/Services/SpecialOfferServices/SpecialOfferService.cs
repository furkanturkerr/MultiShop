using AutoMapper;
using MongoDB.Driver;
using MultiShop.Catalog.Dtos.SpecialOfferDtos;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.SpecialOfferServices;

public class SpecialOfferService : ISpecialOfferService
{
    private readonly IMongoCollection<SpecialOffer> _collection;
    private readonly IMapper _mapper;

    public SpecialOfferService(IMapper mapper, IDatabaseSettings databaseSettings)
    {
        var client = new MongoClient(databaseSettings.ConnectionString);
        var database = client.GetDatabase(databaseSettings.DatabaseName);
        _collection = database.GetCollection<SpecialOffer>(databaseSettings.SpecialOfferCollectionName);
        _mapper = mapper;
    }

    public async Task<List<ResultSpecialOfferDto>> GetAllSpecialOfferAsync()
    {
        var values = await  _collection.Find(x => true).ToListAsync();
        return _mapper.Map<List<ResultSpecialOfferDto>>(values);
    }

    public async Task CreateSpecialOfferAsync(CreateSpecialOfferDto dto)
    {
        var value = _mapper.Map<SpecialOffer>(dto);
        await _collection.InsertOneAsync(value);
    }

    public async Task UpdateSpecialOfferAsync(UpdateSpecialOfferDto dto)
    {
        var value = _mapper.Map<SpecialOffer>(dto);
        await _collection.ReplaceOneAsync(x => x.SpecialOfferId == dto.SpecialOfferId, value);
    }

    public async Task<UpdateSpecialOfferDto> GetSpecialOfferByIdAsync(string id)
    {
        var value = await _collection.Find(x=>x.SpecialOfferId == id).FirstOrDefaultAsync();
        return _mapper.Map<UpdateSpecialOfferDto>(value);
    }

    public async Task DeleteSpecialOfferByIdAsync(string id)
    {
        await _collection.DeleteOneAsync(x=>x.SpecialOfferId == id);
    }

    public async Task<List<ResultSpecialOfferDto>> GetSpecialOfferByStatusAsync()
    {
        var values = await _collection.Find(x => true & x.IsActive == true).ToListAsync();
        return _mapper.Map<List<ResultSpecialOfferDto>>(values);
    }
}