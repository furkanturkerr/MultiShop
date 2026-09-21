using Microsoft.AspNetCore.Mvc;
using MultiShop.Dtos.CatalogDtos.FeaturedDtos;

namespace MultiShop.WebUI.ViewComponents.Home;

public class HomeFeaturedViewComponent : ViewComponent
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HomeFeaturedViewComponent(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync("http://localhost:5053/api/Featured/status");
        if (response.IsSuccessStatusCode)
        {
            var jsonData = await response.Content.ReadFromJsonAsync<List<ResultFeaturedDto>>();
            return View(jsonData);
        }
        return View();
    }
}