using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Dtos.CatalogDtos.OfferDiscountDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[AutoValidateAntiforgeryToken]
[Authorize(Roles = "Admin")]

public class OfferDiscountController : Controller
{
    private readonly IOfferDiscountService _offerDiscountService;

    public OfferDiscountController(IOfferDiscountService offerDiscountService)
    {
        _offerDiscountService = offerDiscountService;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        return View(await _offerDiscountService.GetAllOfferDiscountAsync());
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateOfferDiscountDto dto)
    {
        if (await _offerDiscountService.CreateOfferDiscountAsync(dto))
        {
            return RedirectToAction("Index");
        }
        return View(dto);
    }

    [HttpGet]
    public async Task<IActionResult> Update(string id)
    {
        var value = await _offerDiscountService.GetByIdOfferDiscountAsync(id);
        return value is null ? NotFound() : View(value);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateOfferDiscountDto dto)
    {
        if (await _offerDiscountService.UpdateOfferDiscountAsync(dto))
        {
            return RedirectToAction("Index");
        }
        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        await _offerDiscountService.DeleteOfferDiscountAsync(id);
        return RedirectToAction("Index");
    }
}
