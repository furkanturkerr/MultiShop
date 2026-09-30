using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Dtos.BasketDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly IBasketService _basketService;

    public CartController(IBasketService basketService)
    {
        _basketService = basketService;
    }

    // GET
    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> AddBasketItem(string productId)
    {
        if (string.IsNullOrWhiteSpace(productId))
            return BadRequest();

        if (!await _basketService.AddProductAsync(new AddBasketItemDto { ProductId = productId }))
            return RedirectToAction("Detail", "Product", new { id = productId });

        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddSelectedItem(AddBasketItemDto selection)
    {
        if (!ModelState.IsValid || !await _basketService.AddProductAsync(selection))
        {
            TempData["ProductSelectionError"] = "Ürünün tüm seçeneklerini seçip tekrar deneyin.";
            return RedirectToAction("Detail", "Product", new { id = selection.ProductId });
        }
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> RemoveBasketItem(string basketItemId)
    {
        await _basketService.RemoveBasketItemAsync(basketItemId);
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateQuantity(string basketItemId, int quantity)
    {
        if (string.IsNullOrWhiteSpace(basketItemId) || quantity is < 1 or > 99)
            return BadRequest(new { success = false, message = "Ürün adedi 1 ile 99 arasında olmalıdır." });

        var basket = await _basketService.UpdateBasketItemQuantityAsync(basketItemId, quantity);
        if (basket is null)
            return NotFound(new { success = false, message = "Sepetteki ürün bulunamadı." });

        var basketItem = basket.BasketItems.First(x => x.BasketItemId == basketItemId);

        return Json(new
        {
            success = true,
            quantity = basketItem.Quantity,
            lineTotal = basketItem.ProductPrice * basketItem.Quantity,
            subtotal = basket.TotalPrice,
            discountAmount = basket.DiscountAmount,
            total = basket.TotalPriceAfterDiscount
        });
    }
}
