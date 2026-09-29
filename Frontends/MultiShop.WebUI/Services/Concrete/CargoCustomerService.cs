using MultiShop.Dtos.CargoDtos.CargoCustomerDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Services.Concrete;

public class CargoCustomerService : ICargoCustomerService
{
    private const string Endpoint = "cargo/CargoCustomers";
    private readonly HttpClient _client;

    public CargoCustomerService(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient("GatewayApi");
    }

    public async Task<List<ResultCargoCustomerDto>> GetAllAsync()
    {
        var response = await _client.GetAsync(Endpoint);
        if (!response.IsSuccessStatusCode)
            return [];

        return await response.Content.ReadFromJsonAsync<List<ResultCargoCustomerDto>>() ?? [];
    }

    public async Task<ResultCargoCustomerDto?> GetMyCargoCustomerAsync()
    {
        var response = await _client.GetAsync($"{Endpoint}/me");
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<ResultCargoCustomerDto>();
    }
}
