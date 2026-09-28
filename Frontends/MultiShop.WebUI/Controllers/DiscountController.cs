using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Controllers;

[Authorize]
public class DiscountController : Controller
{
    private readonly IDiscountService _discountService;
    private readonly IBasketService _basketService;

    public DiscountController(IDiscountService discountService, IBasketService basketService)
    {
        _discountService = discountService;
        _basketService = basketService;
    }

    [HttpPost]
    public async Task<IActionResult> Index(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            TempData["DiscountError"] = "İndirim kodunu yazmalısın.";
            return RedirectToAction("Index", "Cart");
        }

        var discount = await _discountService.GetDiscountCodeAsync(code.Trim());
        if (discount is null || !discount.IsActive || discount.ValidDate < DateTime.Now)
        {
            TempData["DiscountError"] = "İndirim kodu geçersiz veya kullanım süresi dolmuş.";
            return RedirectToAction("Index", "Cart");
        }

        var basket = await _basketService.GetBasketAsync();
        basket.DiscountCode = discount.Code;
        basket.DiscountRate = discount.Rate;
        await _basketService.SaveBasketAsync(basket);

        TempData["DiscountSuccess"] = $"%{discount.Rate} indirim uygulandı.";
        return RedirectToAction("Index", "Cart");
    }
}
