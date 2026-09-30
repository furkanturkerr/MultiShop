using MultiShop.Dtos.CatalogDtos.ProductImageDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Services.Concrete;

public class ProductImageService : IProductImageService
{
    private readonly HttpClient _client;

    public ProductImageService(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient("GatewayApi");
    }

    public async Task<UpdateProductImageDto?> GetProductImageByProductIdAsync(string productId)
    {
        var address = $"catalog/ProductImages/ProductImagesByProductId?productId={Uri.EscapeDataString(productId)}";
        var response = await _client.GetAsync(address);

        if (!response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.NoContent)
            return null;

        return await response.Content.ReadFromJsonAsync<UpdateProductImageDto>();
    }

    public async Task<bool> UpdateProductImageAsync(UpdateProductImageDto dto)
    {
        var response = await _client.PutAsJsonAsync("catalog/ProductImages", dto);
        return response.IsSuccessStatusCode;
    }
}
