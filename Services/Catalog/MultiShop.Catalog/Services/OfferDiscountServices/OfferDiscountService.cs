using AutoMapper;
using MongoDB.Driver;
using MultiShop.Catalog.Dtos.OfferDiscountDtos;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.OfferDiscountServices;

public class OfferDiscountService : IOfferDiscountService
{
    private readonly IMongoCollection<OfferDiscount> _collection;
    private readonly IMapper _mapper;

    public OfferDiscountService(IMapper mapper, IDatabaseSettings databaseSettings)
    {
        var client = new MongoClient(databaseSettings.ConnectionString);
        var database = client.GetDatabase(databaseSettings.DatabaseName);
        _collection = database.GetCollection<OfferDiscount>(databaseSettings.OfferDiscountCollectionName);
        _mapper = mapper;
    }

    public async Task<List<ResultOfferDiscountDto>> GetAllOfferDiscountAsync()
    {
        var values = await  _collection.Find(x => true).ToListAsync();
        return _mapper.Map<List<ResultOfferDiscountDto>>(values);
    }

    public async Task CreateOfferDiscountAsync(CreateOfferDiscountDto dto)
    {
        var value = _mapper.Map<OfferDiscount>(dto);
        await _collection.InsertOneAsync(value);
    }

    public async Task UpdateOfferDiscountAsync(UpdateOfferDiscountDto dto)
    {
        var value = _mapper.Map<OfferDiscount>(dto);
        await _collection.ReplaceOneAsync(x => x.OfferDiscountId == dto.OfferDiscountId, value);
    }

    public async Task<UpdateOfferDiscountDto> GetOfferDiscountByIdAsync(string id)
    {
        var value = await _collection.Find(x=>x.OfferDiscountId == id).FirstOrDefaultAsync();
        return _mapper.Map<UpdateOfferDiscountDto>(value);
    }

    public async Task DeleteOfferDiscountByIdAsync(string id)
    {
        await _collection.DeleteOneAsync(x=>x.OfferDiscountId == id);
    }

    public async Task<List<ResultOfferDiscountDto>> GetOfferDiscountByStatusAsync()
    {
        var values = await _collection.Find(x => true & x.IsActive == true).ToListAsync();
        return _mapper.Map<List<ResultOfferDiscountDto>>(values);
    }
}