using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Dtos.CatalogDtos.ProductDetailDtos;

namespace MultiShop.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[AutoValidateAntiforgeryToken]
[Authorize(Roles = "Admin")]

public class ProductDetailController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ProductDetailController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest("Ürün kimliği gereklidir.");
        }

        var client = _httpClientFactory.CreateClient("GatewayApi");
        using var response = await client.GetAsync($"catalog/ProductDetails/ProductDetailsByProductId?productId={Uri.EscapeDataString(id)}");
        if (response.StatusCode == System.Net.HttpStatusCode.NoContent ||
            response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return NotFound("Ürün detayı bulunamadı.");
        }

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, "Ürün detayı yüklenemedi.");
        }

        var dto = await response.Content.ReadFromJsonAsync<UpdateProductDetailDto>();
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

        var client = _httpClientFactory.CreateClient("GatewayApi");
        using var response = await client.PutAsJsonAsync("catalog/ProductDetails", dto);
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "Ürün detayı kaydedilemedi. Bilgileri kontrol edip tekrar dene.");
            return View("Index", dto);
        }
        
        return RedirectToAction("ProductList", "Product", new { area = "Admin" });    }
}
