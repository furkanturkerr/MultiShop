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
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync($"http://localhost:5053/api/ProductImages/ProductImagesByProductId?productId={productId}");
        if (response.IsSuccessStatusCode)
        {
            var jsonData = await response.Content.ReadFromJsonAsync<UpdateProductImageDto>();
            return View(jsonData);
        }
        return View();
    }
}