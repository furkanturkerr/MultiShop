using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Dtos.CatalogDtos.FeaturedDtos;

namespace MultiShop.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[AutoValidateAntiforgeryToken]
[Authorize(Roles = "Admin")]

public class FeaturedController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public FeaturedController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient("GatewayApi");
        var response = await client.GetAsync("catalog/Featured");
        if (response.IsSuccessStatusCode)
        {
            var jsonData = await response.Content.ReadFromJsonAsync<List<ResultFeaturedDto>>();
            return View(jsonData);
        }
        return View();
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateFeaturedDto dto)
    {
        var client = _httpClientFactory.CreateClient("GatewayApi");
        var response = await client.PostAsJsonAsync("catalog/Featured", dto);
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
        var response = await client.GetAsync($"catalog/Featured/{id}");
        if (response.IsSuccessStatusCode)
        {
            var jsonData = await response.Content.ReadFromJsonAsync<UpdateFeaturedDto>();
            return View(jsonData);
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateFeaturedDto dto)
    {
        var client = _httpClientFactory.CreateClient("GatewayApi");
        var response = await client.PutAsJsonAsync($"catalog/Featured", dto);
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
        await client.DeleteAsync($"catalog/Featured/{id}");
        return RedirectToAction("Index");
    }
}