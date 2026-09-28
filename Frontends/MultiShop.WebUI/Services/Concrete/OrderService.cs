using MultiShop.Dtos.OrderDtos.OrderDetailDtos;
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
        var response = await _client.PostAsJsonAsync("order/Orderings", dto);
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<int>();
    }

    public async Task<bool> CreateOrderDetailAsync(CreateOrderDetailDto dto)
    {
        var response = await _client.PostAsJsonAsync("order/OrderDetails", dto);
        return response.IsSuccessStatusCode;
    }
}
