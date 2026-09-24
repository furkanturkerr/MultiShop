using Microsoft.AspNetCore.Mvc;
using MultiShop.Dtos.CatalogDtos.ProductDtos;

namespace MultiShop.WebUI.ViewComponents.Home;

public class HomeProductsViewComponent : ViewComponent
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HomeProductsViewComponent(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var client = _httpClientFactory.CreateClient("GatewayApi");
        var response = await client.GetAsync("catalog/Products");
        if (response.IsSuccessStatusCode)
        {
            var jsonData = await response.Content.ReadFromJsonAsync<List<ResultProductDto>>();
            return View(jsonData);
        }
        return View();
    }
}