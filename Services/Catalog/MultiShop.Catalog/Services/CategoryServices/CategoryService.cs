using AutoMapper;
using MongoDB.Driver;
using MultiShop.Catalog.Dtos.CategoryDtos;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.CategoryServices;

public class CategoryService : ICategoryService
{
    private readonly IMongoCollection<Category> _collection;
    private readonly IMapper _mapper;

    public CategoryService(IMapper mapper, IDatabaseSettings databaseSettings)
    {
        var client = new MongoClient(databaseSettings.ConnectionString);
        var database = client.GetDatabase(databaseSettings.DatabaseName);
        _collection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
        _mapper = mapper;
    }

    public async Task<List<ResultCategoryDto>> GetAllCategoriesAsync()
    {
        var values = await  _collection.Find(x => true).ToListAsync();
        return _mapper.Map<List<ResultCategoryDto>>(values);
    }

    public async Task CreateCategoryAsync(CreateCategoryDto dto)
    {
        var value = _mapper.Map<Category>(dto);
        await _collection.InsertOneAsync(value);
    }

    public async Task UpdateCategoryAsync(UpdateCategoryDto dto)
    {
        var update = Builders<Category>.Update.Set(x => x.CategoryName, dto.CategoryName);
        await _collection.UpdateOneAsync(x => x.CategoryId == dto.CategoryId, update);
    }

    public async Task<GetByIdCategoryDto> GetCategoryByIdAsync(string id)
    {
        var value = await _collection.Find(x=>x.CategoryId == id).FirstOrDefaultAsync();
        return _mapper.Map<GetByIdCategoryDto>(value);
    }

    public async Task DeleteCategoryByIdAsync(string id)
    {
        await _collection.DeleteOneAsync(x=>x.CategoryId == id);
    }
}
