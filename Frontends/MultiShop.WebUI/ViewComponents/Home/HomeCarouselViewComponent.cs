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
        var client = _httpClientFactory.CreateClient("GatewayApi");
        var response = await client.GetAsync("catalog/FeatureSlider/status");
        if (response.IsSuccessStatusCode)
        {
            var jsonData = await response.Content.ReadFromJsonAsync<List<ResultFeatureSliderDto>>();
            return View(jsonData);
        }
        return View();
    }
}