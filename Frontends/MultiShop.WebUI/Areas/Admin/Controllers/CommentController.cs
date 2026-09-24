using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Dtos.CommentDtos;

namespace MultiShop.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[AutoValidateAntiforgeryToken]
[Authorize(Roles = "Admin")]

public class CommentController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CommentController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient("GatewayApi");
        var response = await client.GetAsync("comment/Comments");
        if (response.IsSuccessStatusCode)
        {
            var jsonData = await response.Content.ReadFromJsonAsync<List<ResultCommentDto>>();
            return View(jsonData);
        }
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var client = _httpClientFactory.CreateClient("GatewayApi");
        var response = await client.GetAsync($"comment/Comments/{id}");
        var jsonData = await response.Content.ReadFromJsonAsync<UpdateCommentDto>();
        return View(jsonData);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateCommentDto dto)
    {
        var client = _httpClientFactory.CreateClient("GatewayApi");
        var response = await client.PutAsJsonAsync($"comment/Comments", dto);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var client = _httpClientFactory.CreateClient("GatewayApi");
        using var response = await client.DeleteAsync($"comment/Comments?id={id}");
        response.EnsureSuccessStatusCode();
        return RedirectToAction("Index");
    }
}
