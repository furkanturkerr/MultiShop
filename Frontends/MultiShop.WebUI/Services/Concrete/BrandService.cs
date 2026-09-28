using MultiShop.Dtos.CatalogDtos.BrandDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Services.Concrete;

public class BrandService : IBrandService
{
    private readonly HttpClient _client;

    public BrandService(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient("GatewayApi");
    }

    public async Task<List<ResultBrandDto>> GetAllBrandAsync()
    {
        var response = await _client.GetAsync("catalog/Brand");
        if (!response.IsSuccessStatusCode)
            return new List<ResultBrandDto>();

        return await response.Content.ReadFromJsonAsync<List<ResultBrandDto>>() ?? new List<ResultBrandDto>();
    }

    public async Task<List<ResultBrandDto>> GetActiveBrandAsync()
    {
        var response = await _client.GetAsync("catalog/Brand/status");
        if (!response.IsSuccessStatusCode)
            return new List<ResultBrandDto>();

        return await response.Content.ReadFromJsonAsync<List<ResultBrandDto>>() ?? new List<ResultBrandDto>();
    }

    public async Task<UpdateBrandDto?> GetByIdBrandAsync(string id)
    {
        var response = await _client.GetAsync($"catalog/Brand/{id}");
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<UpdateBrandDto>();
    }

    public async Task<bool> CreateBrandAsync(CreateBrandDto dto)
    {
        var response = await _client.PostAsJsonAsync("catalog/Brand", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateBrandAsync(UpdateBrandDto dto)
    {
        var response = await _client.PutAsJsonAsync("catalog/Brand", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task DeleteBrandAsync(string id)
    {
        await _client.DeleteAsync($"catalog/Brand/{id}");
    }
}
