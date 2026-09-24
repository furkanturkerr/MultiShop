using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Dtos.CatalogDtos.BrandDtos;

namespace MultiShop.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[AutoValidateAntiforgeryToken]
[Authorize(Roles = "Admin")]

public class BrandController : Controller
{
private readonly IHttpClientFactory _httpClientFactory;

    public BrandController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient("GatewayApi");
        var response = await client.GetAsync("catalog/Brand");
        if (response.IsSuccessStatusCode)
        {
            var jsonData = await response.Content.ReadFromJsonAsync<List<ResultBrandDto>>();
            return View(jsonData);
        }
        return View();
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateBrandDto dto)
    {
        var client = _httpClientFactory.CreateClient("GatewayApi");
        var response = await client.PostAsJsonAsync("catalog/Brand", dto);
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
        var response = await client.GetAsync($"catalog/Brand/{id}");
        if (response.IsSuccessStatusCode)
        {
            var jsonData = await response.Content.ReadFromJsonAsync<UpdateBrandDto>();
            return View(jsonData);
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateBrandDto dto)
    {
        var client = _httpClientFactory.CreateClient("GatewayApi");
        var response = await client.PutAsJsonAsync($"catalog/Brand", dto);
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
        await client.DeleteAsync($"catalog/Brand/{id}");
        return RedirectToAction("Index");
    }
}