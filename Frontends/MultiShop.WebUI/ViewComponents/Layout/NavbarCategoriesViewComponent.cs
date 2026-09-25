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
        try
        {
            var client = _httpClientFactory.CreateClient("GatewayApi");
            using var response = await client.GetAsync("catalog/Categories");
            if (response.IsSuccessStatusCode)
                return View(await response.Content.ReadFromJsonAsync<List<ResultCategoryDto>>() ?? new());
        }
        catch (HttpRequestException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        return View(new List<ResultCategoryDto>());
    }
}
