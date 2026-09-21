using Microsoft.AspNetCore.Mvc;
using MultiShop.Dtos.CatalogDtos.BrandDtos;

namespace MultiShop.WebUI.ViewComponents.Home;

public class HomeVendorViewComponent : ViewComponent
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HomeVendorViewComponent(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync("http://localhost:5053/api/Brand/status");
        if (response.IsSuccessStatusCode)
        {
            var jsonData = await response.Content.ReadFromJsonAsync<List<ResultBrandDto>>();
            return View(jsonData);
        }
        return View();
    }
}