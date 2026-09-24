using Microsoft.AspNetCore.Mvc;
using MultiShop.Dtos.CatalogDtos.ProductDtos;
using MultiShop.Dtos.CatalogDtos.ProductImageDtos;

namespace MultiShop.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[AutoValidateAntiforgeryToken]

public class ProductImageController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ProductImageController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // GET
    public async Task<IActionResult> Index(string id)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync($"http://localhost:5053/api/ProductImages/ProductImagesByProductId?productId={id}");
        if (response.IsSuccessStatusCode)
        {
            var jsonData = await response.Content.ReadFromJsonAsync<UpdateProductImageDto>();
            return View(jsonData);
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateProductImageDto dto)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.PutAsJsonAsync($"http://localhost:5053/api/ProductImages", dto);
        return RedirectToAction("ProductList", "Product", "Admin");
    }
}