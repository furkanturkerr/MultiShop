using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Dtos.CatalogDtos.FeatureSliderDtos;

namespace MultiShop.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[AutoValidateAntiforgeryToken]
[Authorize(Roles = "Admin")]

public class FeatureSliderController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public FeatureSliderController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient("GatewayApi");
        var response = await client.GetAsync("catalog/FeatureSlider");
        if (response.IsSuccessStatusCode)
        {
            var jsonData = await response.Content.ReadFromJsonAsync<List<ResultFeatureSliderDto>>();
            return View(jsonData);
        }
        return View();
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateFeatureSliderDto dto)
    {
        var client = _httpClientFactory.CreateClient("GatewayApi");
        var response = await client.PostAsJsonAsync("catalog/FeatureSlider", dto);
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
        var response = await client.GetAsync($"catalog/FeatureSlider/{id}");
        if (response.IsSuccessStatusCode)
        {
            var jsonData = await response.Content.ReadFromJsonAsync<UpdateFeatureSliderDto>();
            return View(jsonData);
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateFeatureSliderDto dto)
    {
        var client = _httpClientFactory.CreateClient("GatewayApi");
        var response = await client.PutAsJsonAsync($"catalog/FeatureSlider", dto);
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
        await client.DeleteAsync($"catalog/FeatureSlider/{id}");
        return RedirectToAction("Index");
    }
}