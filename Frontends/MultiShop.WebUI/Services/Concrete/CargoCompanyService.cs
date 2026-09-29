using MultiShop.Dtos.CargoDtos.CargoCompanyDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Services.Concrete;

public class CargoCompanyService : ICargoCompanyService
{
    private const string Endpoint = "cargo/CargoCompanies";
    private readonly HttpClient _client;

    public CargoCompanyService(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient("GatewayApi");
    }

    public async Task<List<ResultCargoCompanyDto>> GetAllAsync()
    {
        var response = await _client.GetAsync(Endpoint);
        if (!response.IsSuccessStatusCode)
            return [];

        return await response.Content.ReadFromJsonAsync<List<ResultCargoCompanyDto>>() ?? [];
    }

    public async Task<UpdateCargoCompanyDto?> GetByIdAsync(int id)
    {
        var response = await _client.GetAsync($"{Endpoint}/{id}");
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<UpdateCargoCompanyDto>();
    }

    public async Task<bool> CreateAsync(CreateCargoCompanyDto dto)
    {
        var response = await _client.PostAsJsonAsync(Endpoint, dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync(UpdateCargoCompanyDto dto)
    {
        var response = await _client.PutAsJsonAsync(Endpoint, dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _client.DeleteAsync($"{Endpoint}/{id}");
        return response.IsSuccessStatusCode;
    }
}
