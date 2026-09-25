using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Dtos.CatalogDtos.FeatureSliderDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[AutoValidateAntiforgeryToken]
[Authorize(Roles = "Admin")]

public class FeatureSliderController : Controller
{
    private readonly IFeatureSliderService _featureSliderService;

    public FeatureSliderController(IFeatureSliderService featureSliderService)
    {
        _featureSliderService = featureSliderService;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        return View(await _featureSliderService.GetAllFeatureSliderAsync());
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateFeatureSliderDto dto)
    {
        if (await _featureSliderService.CreateFeatureSliderAsync(dto))
        {
            return RedirectToAction("Index");
        }
        return View(dto);
    }

    [HttpGet]
    public async Task<IActionResult> Update(string id)
    {
        var value = await _featureSliderService.GetByIdFeatureSliderAsync(id);
        return value is null ? NotFound() : View(value);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateFeatureSliderDto dto)
    {
        if (await _featureSliderService.UpdateFeatureSliderAsync(dto))
        {
            return RedirectToAction("Index");
        }
        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        await _featureSliderService.DeleteFeatureSliderAsync(id);
        return RedirectToAction("Index");
    }
}
