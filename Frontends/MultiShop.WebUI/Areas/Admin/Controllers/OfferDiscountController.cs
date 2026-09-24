using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Dtos.CatalogDtos.OfferDiscountDtos;

namespace MultiShop.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[AutoValidateAntiforgeryToken]
[Authorize(Roles = "Admin")]

public class OfferDiscountController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public OfferDiscountController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient("GatewayApi");
        var response = await client.GetAsync("catalog/OfferDiscount");
        if (response.IsSuccessStatusCode)
        {
            var jsonData = await response.Content.ReadFromJsonAsync<List<ResultOfferDiscountDto>>();
            return View(jsonData);
        }
        return View();
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateOfferDiscountDto dto)
    {
        var client = _httpClientFactory.CreateClient("GatewayApi");
        var response = await client.PostAsJsonAsync("catalog/OfferDiscount", dto);
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
        var response = await client.GetAsync($"catalog/OfferDiscount/{id}");
        if (response.IsSuccessStatusCode)
        {
            var jsonData = await response.Content.ReadFromJsonAsync<UpdateOfferDiscountDto>();
            return View(jsonData);
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateOfferDiscountDto dto)
    {
        var client = _httpClientFactory.CreateClient("GatewayApi");
        var response = await client.PutAsJsonAsync($"catalog/OfferDiscount", dto);
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
        await client.DeleteAsync($"catalog/OfferDiscount/{id}");
        return RedirectToAction("Index");
    }
}