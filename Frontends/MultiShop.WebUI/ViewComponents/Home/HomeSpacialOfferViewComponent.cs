using Microsoft.AspNetCore.Mvc;
using MultiShop.Dtos.CatalogDtos.SpecialOfferDtos;

namespace MultiShop.WebUI.ViewComponents.Home;

public class HomeSpacialOfferViewComponent : ViewComponent
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HomeSpacialOfferViewComponent(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var client = _httpClientFactory.CreateClient("GatewayApi");
        var response = await client.GetAsync("catalog/SpecialOffer/status");
        if (response.IsSuccessStatusCode)
        {
            var jsonData = await response.Content.ReadFromJsonAsync<List<ResultSpecialOfferDto>>();
            return View(jsonData);
        }
        return View();
    }
}