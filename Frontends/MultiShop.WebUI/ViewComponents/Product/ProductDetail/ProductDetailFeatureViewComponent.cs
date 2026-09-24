using Microsoft.AspNetCore.Mvc;
using MultiShop.Dtos.CatalogDtos.ProductDtos;

namespace MultiShop.WebUI.ViewComponents.Product.ProductDetail;

public class ProductDetailFeatureViewComponent : ViewComponent
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ProductDetailFeatureViewComponent(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IViewComponentResult> InvokeAsync(string id)
    {
        var client = _httpClientFactory.CreateClient("GatewayApi");
        var response = await client.GetAsync($"catalog/Products/{id}");
        if (response.IsSuccessStatusCode)
        {
            var jsonData = await response.Content.ReadFromJsonAsync<UpdateProductDto>();
            return View(jsonData);
        }
        return View();
    }
}