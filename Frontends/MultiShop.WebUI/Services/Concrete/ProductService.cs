using MultiShop.Dtos.CatalogDtos.ProductDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Services.Concrete;

public class ProductService : IProductService
{
    private readonly HttpClient _client;

    public ProductService(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient("GatewayApi");
    }

    public async Task<List<ResultProductDto>> GetAllProductAsync()
    {
        var response = await _client.GetAsync("catalog/Products");
        if (!response.IsSuccessStatusCode)
            return new List<ResultProductDto>();

        return await response.Content.ReadFromJsonAsync<List<ResultProductDto>>() ?? new List<ResultProductDto>();
    }

    public async Task<List<ResultProductDto>> GetProductByCategoryIdAsync(string categoryId)
    {
        var address = $"catalog/Products/CategoryId?categoryId={Uri.EscapeDataString(categoryId)}";
        var response = await _client.GetAsync(address);
        if (!response.IsSuccessStatusCode)
            return new List<ResultProductDto>();

        return await response.Content.ReadFromJsonAsync<List<ResultProductDto>>() ?? new List<ResultProductDto>();
    }

    public async Task<UpdateProductDto?> GetByIdProductAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return null;

        var response = await _client.GetAsync($"catalog/Products/{id}");
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<UpdateProductDto>();
    }

    public async Task<bool> CreateProductAsync(CreateProductDto dto)
    {
        var response = await _client.PostAsJsonAsync("catalog/Products", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateProductAsync(UpdateProductDto dto)
    {
        var response = await _client.PutAsJsonAsync("catalog/Products", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task DeleteProductAsync(string id)
    {
        await _client.DeleteAsync($"catalog/Products/{id}");
    }
}
