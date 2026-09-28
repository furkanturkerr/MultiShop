using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Dtos.DiscountDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[AutoValidateAntiforgeryToken]
[Authorize(Roles = "Admin,Manager")]
public class CouponController : Controller
{
    private readonly IDiscountService _discountService;

    public CouponController(IDiscountService discountService)
    {
        _discountService = discountService;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _discountService.GetAllCouponsAsync());
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new CreateCouponDto
        {
            IsActive = true,
            ValidDate = DateTime.Today.AddDays(30)
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCouponDto dto)
    {
        ValidateDate(dto.ValidDate);
        if (!ModelState.IsValid)
            return View(dto);

        dto.Code = dto.Code.Trim().ToUpperInvariant();
        if (!await _discountService.CreateCouponAsync(dto))
        {
            ModelState.AddModelError(string.Empty, "Kupon kaydedilemedi.");
            return View(dto);
        }

        TempData["CouponSuccess"] = "Kupon oluşturuldu.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var coupon = await _discountService.GetCouponByIdAsync(id);
        return coupon is null ? NotFound() : View(coupon);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateCouponDto dto)
    {
        ValidateDate(dto.ValidDate);
        if (!ModelState.IsValid)
            return View(dto);

        dto.Code = dto.Code.Trim().ToUpperInvariant();
        if (!await _discountService.UpdateCouponAsync(dto))
        {
            ModelState.AddModelError(string.Empty, "Kupon güncellenemedi.");
            return View(dto);
        }

        TempData["CouponSuccess"] = "Kupon güncellendi.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _discountService.DeleteCouponAsync(id);
        TempData[deleted ? "CouponSuccess" : "CouponError"] =
            deleted ? "Kupon silindi." : "Kupon silinemedi.";
        return RedirectToAction(nameof(Index));
    }

    private void ValidateDate(DateTime validDate)
    {
        if (validDate <= DateTime.Now)
            ModelState.AddModelError(nameof(CreateCouponDto.ValidDate), "Geçerlilik tarihi ileri bir tarih olmalıdır.");
    }
}
