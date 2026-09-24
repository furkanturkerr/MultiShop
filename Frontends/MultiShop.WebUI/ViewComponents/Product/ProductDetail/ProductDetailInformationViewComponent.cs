using Microsoft.AspNetCore.Mvc;
using MultiShop.Dtos.CatalogDtos.ProductDetailDtos;

namespace MultiShop.WebUI.ViewComponents.Product.ProductDetail;

public class ProductDetailInformationViewComponent : ViewComponent
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ProductDetailInformationViewComponent(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IViewComponentResult> InvokeAsync(string id)
    {
        var client = _httpClientFactory.CreateClient("GatewayApi");
        var response = await client.GetAsync($"catalog/ProductDetails/ProductDetailsByProductId?productId={id}");
        if (response.IsSuccessStatusCode)
        {
            var jsonData = await response.Content.ReadFromJsonAsync<ResultProductDetailDto>();
            return View(jsonData);
        }
        return View();
    }
}