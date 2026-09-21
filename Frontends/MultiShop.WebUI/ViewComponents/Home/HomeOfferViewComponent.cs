using Microsoft.AspNetCore.Mvc;
using MultiShop.Dtos.CatalogDtos.OfferDiscountDtos;

namespace MultiShop.WebUI.ViewComponents.Home;

public class HomeOfferViewComponent : ViewComponent
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HomeOfferViewComponent(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync("http://localhost:5053/api/OfferDiscount/status");
        if (response.IsSuccessStatusCode)
        {
            var jsonData = await response.Content.ReadFromJsonAsync<List<ResultOfferDiscountDto>>();
            return View(jsonData);
        }
        return View();
    }
}