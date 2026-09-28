using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.ViewComponents.Home;

public class HomeCategoriesViewComponent : ViewComponent
{
    private readonly ICategoryService _categoryService;

    public HomeCategoriesViewComponent(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View(await _categoryService.GetAllCategoryAsync());
    }
}
