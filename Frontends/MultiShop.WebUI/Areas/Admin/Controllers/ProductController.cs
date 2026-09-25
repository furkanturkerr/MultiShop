using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MultiShop.Dtos.CatalogDtos.CategoryDtos;
using MultiShop.Dtos.CatalogDtos.ProductDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[AutoValidateAntiforgeryToken]
[Authorize(Roles = "Admin")]

public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    public ProductController(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    // GET
    public async Task<IActionResult> ProductList()
    {
        var values = await _productService.GetAllProductAsync();
        return View(values);
    }

    public async Task<IActionResult> Create()
    {
        var categories = await _categoryService.GetAllCategoryAsync();
        ViewBag.Category = new SelectList(categories, "CategoryId", "CategoryName");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductDto dto)
    {
        if (await _productService.CreateProductAsync(dto))
        {
            return RedirectToAction("ProductList");
        }
        ViewBag.Category = new SelectList(await _categoryService.GetAllCategoryAsync(), "CategoryId", "CategoryName");
        return View(dto);
    }

    [HttpGet]
    public async Task<IActionResult> Update(string id)
    {
        var value = await _productService.GetByIdProductAsync(id);
        if (value is null)
            return NotFound();

        ViewBag.Category = new SelectList(await _categoryService.GetAllCategoryAsync(),
            "CategoryId", "CategoryName", value.CategoryId);
        return View(value);
    }
    
    [HttpPost]
    public async Task<IActionResult> Update(UpdateProductDto dto)
    {
        if (await _productService.UpdateProductAsync(dto))
        {
            return RedirectToAction("ProductList");
        }
        ViewBag.Category = new SelectList(await _categoryService.GetAllCategoryAsync(),
            "CategoryId", "CategoryName", dto.CategoryId);
        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        await _productService.DeleteProductAsync(id);
        return RedirectToAction("ProductList");
    }
}
