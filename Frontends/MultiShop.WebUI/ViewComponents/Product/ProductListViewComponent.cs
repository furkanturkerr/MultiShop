using Microsoft.AspNetCore.Mvc;
using MultiShop.Dtos.CatalogDtos.ProductDtos;

namespace MultiShop.WebUI.ViewComponents.Product;

public class ProductListViewComponent : ViewComponent
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ProductListViewComponent(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IViewComponentResult> InvokeAsync(string categoryId)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync($"http://localhost:5053/api/Products/CategoryId?categoryId={categoryId}");
        if (response.IsSuccessStatusCode)
        {
            var jsonData = await response.Content.ReadFromJsonAsync<List<ResultProductDto>>();
            return View(jsonData ?? new List<ResultProductDto>());
        }
        return View();
    }
}