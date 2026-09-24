using Microsoft.AspNetCore.Mvc;
using MultiShop.Dtos.CatalogDtos.CategoryDtos;
using MultiShop.WebUI.Infrastructure.Gateway;

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
        catch (GatewayApiException exception) when (exception.StatusCode != System.Net.HttpStatusCode.Unauthorized ||
                                                   HttpContext.User.Identity?.IsAuthenticated != true)
        {
            // The shared header must also render on login/register and when a service is unavailable.
        }
        return View(new List<ResultCategoryDto>());
    }
}
