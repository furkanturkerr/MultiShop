using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Dtos.CatalogDtos.ProductDetailDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[AutoValidateAntiforgeryToken]
[Authorize(Roles = "Admin")]

public class ProductDetailController : Controller
{
    private readonly IProductDetailService _productDetailService;

    public ProductDetailController(IProductDetailService productDetailService)
    {
        _productDetailService = productDetailService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest("Ürün kimliği gereklidir.");
        }

        var dto = await _productDetailService.GetProductDetailByProductIdAsync(id);
        if (dto is null)
        {
            return NotFound("Ürün detayı bulunamadı.");
        }

        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateProductDetailDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View("Index", dto);
        }

        if (!await _productDetailService.UpdateProductDetailAsync(dto))
        {
            ModelState.AddModelError(string.Empty, "Ürün detayı kaydedilemedi. Bilgileri kontrol edip tekrar dene.");
            return View("Index", dto);
        }
        
        return RedirectToAction("ProductList", "Product", new { area = "Admin" });
    }
}
