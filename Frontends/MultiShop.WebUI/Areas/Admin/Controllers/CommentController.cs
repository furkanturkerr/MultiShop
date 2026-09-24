using Microsoft.AspNetCore.Mvc;
using MultiShop.Dtos.CommentDtos;

namespace MultiShop.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[AutoValidateAntiforgeryToken]

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
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync("http://localhost:5025/api/Comments");
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
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync($"http://localhost:5025/api/Comments/{id}");
        var jsonData = await response.Content.ReadFromJsonAsync<UpdateCommentDto>();
        return View(jsonData);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateCommentDto dto)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.PutAsJsonAsync($"http://localhost:5025/api/Comments", dto);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var client = _httpClientFactory.CreateClient();
        await client.DeleteAsync($"http://localhost:5025/api/Comments/{id}");
        return RedirectToAction("Index");
    }
}