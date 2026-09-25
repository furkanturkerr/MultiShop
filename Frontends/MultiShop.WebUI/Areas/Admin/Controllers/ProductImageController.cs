using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Dtos.CatalogDtos.ProductImageDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[AutoValidateAntiforgeryToken]
[Authorize(Roles = "Admin")]

public class ProductImageController : Controller
{
    private readonly IProductImageService _productImageService;

    public ProductImageController(IProductImageService productImageService)
    {
        _productImageService = productImageService;
    }

    // GET
    public async Task<IActionResult> Index(string id)
    {
        var value = await _productImageService.GetProductImageByProductIdAsync(id);
        return value is null ? NotFound() : View(value);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateProductImageDto dto)
    {
        await _productImageService.UpdateProductImageAsync(dto);
        return RedirectToAction("ProductList", "Product", "Admin");
    }
}
