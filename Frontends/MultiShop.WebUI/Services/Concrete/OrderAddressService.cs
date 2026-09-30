using MultiShop.Dtos.OrderDtos.OrderAddressDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Services.Concrete;

public class OrderAddressService : IOrderAddressService
{
    private readonly HttpClient _client;

    public OrderAddressService(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient("GatewayApi");
    }

    public async Task<List<ResultOrderAddressDto>> GetAllOrderAddressesAsync()
    {
        var response = await _client.GetAsync("order/Addresses");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<ResultOrderAddressDto>>() ?? [];
    }

    public async Task<UpdateOrderAddressDto?> GetByIdOrderAddressAsync(int id)
    {
        var response = await _client.GetAsync($"order/Addresses/{id}");
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<UpdateOrderAddressDto>();
    }

    public async Task<int?> CreateOrderAddressAsync(CreateOrderAddressDto dto)
    {
        var response = await _client.PostAsJsonAsync("order/Addresses", dto);
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<int>();
    }

    public async Task<bool> UpdateOrderAddressAsync(UpdateOrderAddressDto dto)
    {
        var response = await _client.PutAsJsonAsync("order/Addresses", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteOrderAddressAsync(int id)
    {
        var response = await _client.DeleteAsync($"order/Addresses?id={id}");
        return response.IsSuccessStatusCode;
    }
}
