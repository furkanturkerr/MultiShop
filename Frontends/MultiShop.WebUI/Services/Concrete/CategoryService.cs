using MultiShop.Dtos.CatalogDtos.CategoryDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Services.Concrete;

public class CategoryService : ICategoryService
{
    private readonly HttpClient _client;

    public CategoryService(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient("GatewayApi");
    }

    public async Task<List<ResultCategoryDto>> GetAllCategoryAsync()
    {
        var response = await _client.GetAsync("catalog/Categories");
        if (!response.IsSuccessStatusCode)
            return new List<ResultCategoryDto>();

        return await response.Content.ReadFromJsonAsync<List<ResultCategoryDto>>() ?? new List<ResultCategoryDto>();
    }

    public async Task<UpdateCategoryDto?> GetByIdCategoryAsync(string id)
    {
        var response = await _client.GetAsync($"catalog/Categories/{id}");
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<UpdateCategoryDto>();
    }

    public async Task<bool> CreateCategoryAsync(CreateCategoryDto dto)
    {
        var response = await _client.PostAsJsonAsync("catalog/Categories", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateCategoryAsync(UpdateCategoryDto dto)
    {
        var response = await _client.PutAsJsonAsync("catalog/Categories", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task DeleteCategoryAsync(string id)
    {
        await _client.DeleteAsync($"catalog/Categories/{id}");
    }
}
