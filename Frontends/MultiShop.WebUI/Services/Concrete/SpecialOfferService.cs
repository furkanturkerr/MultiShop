using MultiShop.Dtos.CatalogDtos.SpecialOfferDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Services.Concrete;

public class SpecialOfferService : ISpecialOfferService
{
    private readonly HttpClient _client;

    public SpecialOfferService(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient("GatewayApi");
    }

    public async Task<List<ResultSpecialOfferDto>> GetAllSpecialOfferAsync()
    {
        var response = await _client.GetAsync("catalog/SpecialOffer");
        if (!response.IsSuccessStatusCode)
            return new List<ResultSpecialOfferDto>();

        return await response.Content.ReadFromJsonAsync<List<ResultSpecialOfferDto>>() ?? new List<ResultSpecialOfferDto>();
    }

    public async Task<List<ResultSpecialOfferDto>> GetActiveSpecialOfferAsync()
    {
        var response = await _client.GetAsync("catalog/SpecialOffer/status");
        if (!response.IsSuccessStatusCode)
            return new List<ResultSpecialOfferDto>();

        return await response.Content.ReadFromJsonAsync<List<ResultSpecialOfferDto>>() ?? new List<ResultSpecialOfferDto>();
    }

    public async Task<UpdateSpecialOfferDto?> GetByIdSpecialOfferAsync(string id)
    {
        var response = await _client.GetAsync($"catalog/SpecialOffer/{id}");
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<UpdateSpecialOfferDto>();
    }

    public async Task<bool> CreateSpecialOfferAsync(CreateSpecialOfferDto dto)
    {
        var response = await _client.PostAsJsonAsync("catalog/SpecialOffer", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateSpecialOfferAsync(UpdateSpecialOfferDto dto)
    {
        var response = await _client.PutAsJsonAsync("catalog/SpecialOffer", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task DeleteSpecialOfferAsync(string id)
    {
        await _client.DeleteAsync($"catalog/SpecialOffer/{id}");
    }
}
