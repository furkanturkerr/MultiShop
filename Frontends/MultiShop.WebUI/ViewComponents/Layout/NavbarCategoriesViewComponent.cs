using Microsoft.AspNetCore.Mvc;
using MultiShop.Dtos.CatalogDtos.CategoryDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.ViewComponents.Layout;

public class NavbarCategoriesViewComponent : ViewComponent
{
    private readonly ICategoryService _categoryService;

    public NavbarCategoriesViewComponent(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        try
        {
            return View(await _categoryService.GetAllCategoryAsync());
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
