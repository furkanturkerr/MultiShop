using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MultiShop.Dtos.CatalogDtos.CategoryDtos;
using MultiShop.Dtos.CatalogDtos.ProductDtos;

namespace MultiShop.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[AutoValidateAntiforgeryToken]

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
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync("http://localhost:5053/api/Products");
        if (response.IsSuccessStatusCode)
        {
            var jsonData = await response.Content.ReadFromJsonAsync<List<ResultProductDto>>();
            return View(jsonData);
        }
        return View();
    }

    public async Task<IActionResult> Create()
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync("http://localhost:5053/api/Categories");
        ViewBag.Category = new SelectList(await response.Content.ReadFromJsonAsync<List<ResultCategoryDto>>(),
            "CategoryId", "CategoryName");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductDto dto)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.PostAsJsonAsync("http://localhost:5053/api/Products", dto);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("ProductList");
        }
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Update(string id)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync($"http://localhost:5053/api/Products/{id}");
        var jsonData = await response.Content.ReadFromJsonAsync<UpdateProductDto>();
        ViewBag.Category = new SelectList(await client.GetFromJsonAsync<List<ResultCategoryDto>>("http://localhost:5053/api/Categories"),
            "CategoryId", "CategoryName", jsonData.CategoryId);
        return View(jsonData);
    }
    
    [HttpPost]
    public async Task<IActionResult> Update(UpdateProductDto dto)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.PutAsJsonAsync($"http://localhost:5053/api/Products", dto);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("ProductList");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        var client = _httpClientFactory.CreateClient();
        await client.DeleteAsync($"http://localhost:5053/api/Products/{id}");
        return RedirectToAction("ProductList");
    }
}