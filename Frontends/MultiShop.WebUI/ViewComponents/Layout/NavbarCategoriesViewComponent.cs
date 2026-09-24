using Microsoft.AspNetCore.Mvc;
using MultiShop.Dtos.CatalogDtos.CategoryDtos;

namespace MultiShop.WebUI.ViewComponents.Layout;

public class NavbarCategoriesViewComponent : ViewComponent
{
    private readonly IHttpClientFactory _httpClientFactory;

    public NavbarCategoriesViewComponent(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync("http://localhost:5053/api/Categories");
        if (response.IsSuccessStatusCode)
        {
            var jsonData = await response.Content.ReadFromJsonAsync<List<ResultCategoryDto>>();
            return View(jsonData);
        }
        return View();
    }
}