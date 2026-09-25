using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Dtos.CatalogDtos.SpecialOfferDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[AutoValidateAntiforgeryToken]
[Authorize(Roles = "Admin")]

public class SpecialOfferController : Controller
{
    private readonly ISpecialOfferService _specialOfferService;

    public SpecialOfferController(ISpecialOfferService specialOfferService)
    {
        _specialOfferService = specialOfferService;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        return View(await _specialOfferService.GetAllSpecialOfferAsync());
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateSpecialOfferDto dto)
    {
        if (await _specialOfferService.CreateSpecialOfferAsync(dto))
        {
            return RedirectToAction("Index");
        }
        return View(dto);
    }

    [HttpGet]
    public async Task<IActionResult> Update(string id)
    {
        var value = await _specialOfferService.GetByIdSpecialOfferAsync(id);
        return value is null ? NotFound() : View(value);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateSpecialOfferDto dto)
    {
        if (await _specialOfferService.UpdateSpecialOfferAsync(dto))
        {
            return RedirectToAction("Index");
        }
        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        await _specialOfferService.DeleteSpecialOfferAsync(id);
        return RedirectToAction("Index");
    }
}
