using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Dtos.CatalogDtos.FeaturedDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[AutoValidateAntiforgeryToken]
[Authorize(Roles = "Admin")]

public class FeaturedController : Controller
{
    private readonly IFeaturedService _featuredService;

    public FeaturedController(IFeaturedService featuredService)
    {
        _featuredService = featuredService;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        return View(await _featuredService.GetAllFeaturedAsync());
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateFeaturedDto dto)
    {
        if (await _featuredService.CreateFeaturedAsync(dto))
        {
            return RedirectToAction("Index");
        }
        return View(dto);
    }

    [HttpGet]
    public async Task<IActionResult> Update(string id)
    {
        var value = await _featuredService.GetByIdFeaturedAsync(id);
        return value is null ? NotFound() : View(value);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateFeaturedDto dto)
    {
        if (await _featuredService.UpdateFeaturedAsync(dto))
        {
            return RedirectToAction("Index");
        }
        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        await _featuredService.DeleteFeaturedAsync(id);
        return RedirectToAction("Index");
    }
}
