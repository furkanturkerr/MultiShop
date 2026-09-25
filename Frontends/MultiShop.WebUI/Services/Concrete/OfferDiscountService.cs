using MultiShop.Dtos.CatalogDtos.OfferDiscountDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Services.Concrete;

public class OfferDiscountService : IOfferDiscountService
{
    private readonly HttpClient _client;

    public OfferDiscountService(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient("GatewayApi");
    }

    public async Task<List<ResultOfferDiscountDto>> GetAllOfferDiscountAsync()
    {
        var response = await _client.GetAsync("catalog/OfferDiscount");
        if (!response.IsSuccessStatusCode)
            return new List<ResultOfferDiscountDto>();

        return await response.Content.ReadFromJsonAsync<List<ResultOfferDiscountDto>>() ?? new List<ResultOfferDiscountDto>();
    }

    public async Task<UpdateOfferDiscountDto?> GetByIdOfferDiscountAsync(string id)
    {
        var response = await _client.GetAsync($"catalog/OfferDiscount/{id}");
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<UpdateOfferDiscountDto>();
    }

    public async Task<bool> CreateOfferDiscountAsync(CreateOfferDiscountDto dto)
    {
        var response = await _client.PostAsJsonAsync("catalog/OfferDiscount", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateOfferDiscountAsync(UpdateOfferDiscountDto dto)
    {
        var response = await _client.PutAsJsonAsync("catalog/OfferDiscount", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task DeleteOfferDiscountAsync(string id)
    {
        await _client.DeleteAsync($"catalog/OfferDiscount/{id}");
    }
}
