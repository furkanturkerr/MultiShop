using Microsoft.AspNetCore.Mvc;
using MultiShop.Dtos.CatalogDtos.FeatureSliderDtos;

namespace MultiShop.WebUI.ViewComponents.Home;

public class HomeCarouselViewComponent : ViewComponent
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HomeCarouselViewComponent(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync("http://localhost:5053/api/FeatureSlider/status");
        if (response.IsSuccessStatusCode)
        {
            var jsonData = await response.Content.ReadFromJsonAsync<List<ResultFeatureSliderDto>>();
            return View(jsonData);
        }
        return View();
    }
}