using MultiShop.Dtos.CatalogDtos.ProductDetailDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Services.Concrete;

public class ProductDetailService : IProductDetailService
{
    private readonly HttpClient _client;

    public ProductDetailService(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient("GatewayApi");
    }

    public async Task<UpdateProductDetailDto?> GetProductDetailByProductIdAsync(string productId)
    {
        var address = $"catalog/ProductDetails/ProductDetailsByProductId?productId={Uri.EscapeDataString(productId)}";
        var response = await _client.GetAsync(address);

        if (!response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.NoContent)
            return null;

        return await response.Content.ReadFromJsonAsync<UpdateProductDetailDto>();
    }

    public async Task<ResultProductDetailDto?> GetResultProductDetailByProductIdAsync(string productId)
    {
        var address = $"catalog/ProductDetails/ProductDetailsByProductId?productId={Uri.EscapeDataString(productId)}";
        var response = await _client.GetAsync(address);

        if (!response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.NoContent)
            return null;

        return await response.Content.ReadFromJsonAsync<ResultProductDetailDto>();
    }

    public async Task<bool> UpdateProductDetailAsync(UpdateProductDetailDto dto)
    {
        var response = await _client.PutAsJsonAsync("catalog/ProductDetails", dto);
        return response.IsSuccessStatusCode;
    }
}
