using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MultiShop.Dtos.CatalogDtos.CategoryDtos;
using MultiShop.Dtos.CatalogDtos.ProductDtos;

namespace MultiShop.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[AutoValidateAntiforgeryToken]
[Authorize(Roles = "Admin")]

public class ProductController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ProductController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // GET
    public async Task<IActionResult> ProductList()
    {
        var client = _httpClientFactory.CreateClient("GatewayApi");
        var response = await client.GetAsync("catalog/Products");
        if (response.IsSuccessStatusCode)
        {
            var jsonData = await response.Content.ReadFromJsonAsync<List<ResultProductDto>>();
            return View(jsonData);
        }
        return View();
    }

    public async Task<IActionResult> Create()
    {
        var client = _httpClientFactory.CreateClient("GatewayApi");
        var response = await client.GetAsync("catalog/Categories");
        ViewBag.Category = new SelectList(await response.Content.ReadFromJsonAsync<List<ResultCategoryDto>>(),
            "CategoryId", "CategoryName");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductDto dto)
    {
        var client = _httpClientFactory.CreateClient("GatewayApi");
        var response = await client.PostAsJsonAsync("catalog/Products", dto);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("ProductList");
        }
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Update(string id)
    {
        var client = _httpClientFactory.CreateClient("GatewayApi");
        var response = await client.GetAsync($"catalog/Products/{id}");
        var jsonData = await response.Content.ReadFromJsonAsync<UpdateProductDto>();
        ViewBag.Category = new SelectList(await client.GetFromJsonAsync<List<ResultCategoryDto>>("catalog/Categories"),
            "CategoryId", "CategoryName", jsonData.CategoryId);
        return View(jsonData);
    }
    
    [HttpPost]
    public async Task<IActionResult> Update(UpdateProductDto dto)
    {
        var client = _httpClientFactory.CreateClient("GatewayApi");
        var response = await client.PutAsJsonAsync($"catalog/Products", dto);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("ProductList");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        var client = _httpClientFactory.CreateClient("GatewayApi");
        await client.DeleteAsync($"catalog/Products/{id}");
        return RedirectToAction("ProductList");
    }
}