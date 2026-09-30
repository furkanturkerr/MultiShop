using MultiShop.Dtos.OrderDtos.OrderingDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Services.Concrete;

public class OrderService : IOrderService
{
    private readonly HttpClient _client;

    public OrderService(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient("GatewayApi");
    }

    public async Task<int?> CreateOrderingAsync(CreateOrderingDto dto)
    {
        using var response = await _client.PostAsJsonAsync("order/Orderings", dto);
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<int>();
    }

    public async Task<List<ResultOrderByUserIdDto>> GetMyOrdersAsync()
    {
        var response =
            await _client.GetFromJsonAsync<List<ResultOrderByUserIdDto>>("order/Orderings/my");

        if (response == null)
            return new List<ResultOrderByUserIdDto>();

        return response;
    }

    public async Task<List<ResultOrderingDto>> GetAllOrdersAsync()
    {
        return await _client.GetFromJsonAsync<List<ResultOrderingDto>>("order/Orderings") ?? [];
    }

    public async Task<ResultOrderingDto?> GetOrderDetailAsync(int id)
    {
        using var response = await _client.GetAsync($"order/Orderings/admin/{id}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ResultOrderingDto>();
    }
}
