using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Dtos.CatalogDtos.CategoryDtos;

namespace MultiShop.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[AutoValidateAntiforgeryToken]
[Authorize(Roles = "Admin,Manager")]

public class CategoryController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CategoryController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient("GatewayApi");
        var response = await client.GetAsync("catalog/Categories");
        if (response.IsSuccessStatusCode)
        {
            var jsonData = await response.Content.ReadFromJsonAsync<List<ResultCategoryDto>>();
            return View(jsonData);
        }
        return View();
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryDto dto)
    {
        var client = _httpClientFactory.CreateClient("GatewayApi");
        var response = await client.PostAsJsonAsync("catalog/Categories", dto);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index");
        }
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Update(string id)
    {
        var client = _httpClientFactory.CreateClient("GatewayApi");
        var response = await client.GetAsync($"catalog/Categories/{id}");
        if (response.IsSuccessStatusCode)
        {
            var jsonData = await response.Content.ReadFromJsonAsync<UpdateCategoryDto>();
            return View(jsonData);
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateCategoryDto dto)
    {
        var client = _httpClientFactory.CreateClient("GatewayApi");
        var response = await client.PutAsJsonAsync($"catalog/Categories", dto);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        var client = _httpClientFactory.CreateClient("GatewayApi");
        await client.DeleteAsync($"catalog/Categories/{id}");
        return RedirectToAction("Index");
    }
}