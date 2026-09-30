using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Dtos.CatalogDtos.ProductDtos;
using MultiShop.WebUI.Models.Product;
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

    public async Task<IActionResult> ProductList([FromQuery] ProductFilterDto filters)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var model = await _productService.GetProductListAsync(filters);
        return View(model);
    }

    public async Task<IActionResult> Create()
    {
        var model = new CreateProductDto();
        await PrepareFormAsync(model.CategoryId, model.Options);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductDto dto)
    {
        if (ModelState.IsValid && await _productService.CreateProductAsync(dto))
        {
            return RedirectToAction("ProductList");
        }

        if (ModelState.IsValid)
            ModelState.AddModelError(string.Empty, "Ürün kaydedilemedi. Lütfen tekrar deneyin.");

        await PrepareFormAsync(dto.CategoryId, dto.Options);
        return View(dto);
    }

    [HttpGet]
    public async Task<IActionResult> Update(string id)
    {
        var value = await _productService.GetByIdProductAsync(id);
        if (value is null)
            return NotFound();

        await PrepareFormAsync(value.CategoryId, value.Options);
        return View(value);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateProductDto dto)
    {
        if (ModelState.IsValid && await _productService.UpdateProductAsync(dto))
        {
            return RedirectToAction("ProductList");
        }

        if (ModelState.IsValid)
            ModelState.AddModelError(string.Empty, "Ürün güncellenemedi. Lütfen tekrar deneyin.");

        await PrepareFormAsync(dto.CategoryId, dto.Options);
        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        await _productService.DeleteProductAsync(id);
        return RedirectToAction("ProductList");
    }

    private async Task PrepareFormAsync(string categoryId, List<ProductOptionDto> options)
    {
        var categories = await _categoryService.GetAllCategoryAsync();
        ViewData["ProductOptions"] = new ProductOptionsViewModel
        {
            CategoryId = categoryId,
            Categories = categories,
            Options = options
        };
    }
}
