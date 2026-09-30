using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Dtos.CatalogDtos.CategoryDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[AutoValidateAntiforgeryToken]
[Authorize(Roles = "Admin,Manager")]

public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        var values = await _categoryService.GetAllCategoryAsync();
        return View(values);
    }

    public IActionResult Create()
    {
        return View(new CreateCategoryDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryDto dto)
    {
        if (ModelState.IsValid && await _categoryService.CreateCategoryAsync(dto))
        {
            return RedirectToAction("Index");
        }
        if (ModelState.IsValid)
            ModelState.AddModelError(string.Empty, "Kategori kaydedilemedi. Lütfen tekrar deneyin.");
        return View(dto);
    }

    [HttpGet]
    public async Task<IActionResult> Update(string id)
    {
        var value = await _categoryService.GetByIdCategoryAsync(id);
        return value is null ? NotFound() : View(value);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateCategoryDto dto)
    {
        if (ModelState.IsValid && await _categoryService.UpdateCategoryAsync(dto))
        {
            return RedirectToAction("Index");
        }
        if (ModelState.IsValid)
            ModelState.AddModelError(string.Empty, "Kategori kaydedilemedi. Lütfen tekrar deneyin.");
        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        await _categoryService.DeleteCategoryAsync(id);
        return RedirectToAction("Index");
    }
}
