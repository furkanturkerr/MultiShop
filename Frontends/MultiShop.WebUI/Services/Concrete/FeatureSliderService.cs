using MultiShop.Dtos.CatalogDtos.FeatureSliderDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Services.Concrete;

public class FeatureSliderService : IFeatureSliderService
{
    private readonly HttpClient _client;

    public FeatureSliderService(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient("GatewayApi");
    }

    public async Task<List<ResultFeatureSliderDto>> GetAllFeatureSliderAsync()
    {
        var response = await _client.GetAsync("catalog/FeatureSlider");
        if (!response.IsSuccessStatusCode)
            return new List<ResultFeatureSliderDto>();

        return await response.Content.ReadFromJsonAsync<List<ResultFeatureSliderDto>>() ?? new List<ResultFeatureSliderDto>();
    }

    public async Task<UpdateFeatureSliderDto?> GetByIdFeatureSliderAsync(string id)
    {
        var response = await _client.GetAsync($"catalog/FeatureSlider/{id}");
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<UpdateFeatureSliderDto>();
    }

    public async Task<bool> CreateFeatureSliderAsync(CreateFeatureSliderDto dto)
    {
        var response = await _client.PostAsJsonAsync("catalog/FeatureSlider", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateFeatureSliderAsync(UpdateFeatureSliderDto dto)
    {
        var response = await _client.PutAsJsonAsync("catalog/FeatureSlider", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task DeleteFeatureSliderAsync(string id)
    {
        await _client.DeleteAsync($"catalog/FeatureSlider/{id}");
    }
}
