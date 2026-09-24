using Microsoft.AspNetCore.Mvc;
using MultiShop.Dtos.CatalogDtos.ProductImageDtos;

namespace MultiShop.WebUI.ViewComponents.Product.ProductDetail;

public class ProductDetailImagesViewComponent : ViewComponent
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ProductDetailImagesViewComponent(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IViewComponentResult> InvokeAsync(string productId)
    {
        var client = _httpClientFactory.CreateClient("GatewayApi");
        var response = await client.GetAsync($"catalog/ProductImages/ProductImagesByProductId?productId={productId}");
        if (response.IsSuccessStatusCode)
        {
            var jsonData = await response.Content.ReadFromJsonAsync<UpdateProductImageDto>();
            return View(jsonData);
        }
        return View();
    }
}