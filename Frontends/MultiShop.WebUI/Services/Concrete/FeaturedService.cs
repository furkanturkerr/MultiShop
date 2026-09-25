using MultiShop.Dtos.CatalogDtos.FeaturedDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Services.Concrete;

public class FeaturedService : IFeaturedService
{
    private readonly HttpClient _client;

    public FeaturedService(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient("GatewayApi");
    }

    public async Task<List<ResultFeaturedDto>> GetAllFeaturedAsync()
    {
        var response = await _client.GetAsync("catalog/Featured");
        if (!response.IsSuccessStatusCode)
            return new List<ResultFeaturedDto>();

        return await response.Content.ReadFromJsonAsync<List<ResultFeaturedDto>>() ?? new List<ResultFeaturedDto>();
    }

    public async Task<UpdateFeaturedDto?> GetByIdFeaturedAsync(string id)
    {
        var response = await _client.GetAsync($"catalog/Featured/{id}");
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<UpdateFeaturedDto>();
    }

    public async Task<bool> CreateFeaturedAsync(CreateFeaturedDto dto)
    {
        var response = await _client.PostAsJsonAsync("catalog/Featured", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateFeaturedAsync(UpdateFeaturedDto dto)
    {
        var response = await _client.PutAsJsonAsync("catalog/Featured", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task DeleteFeaturedAsync(string id)
    {
        await _client.DeleteAsync($"catalog/Featured/{id}");
    }
}
