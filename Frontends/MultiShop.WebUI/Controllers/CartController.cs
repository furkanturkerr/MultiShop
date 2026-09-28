using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Dtos.BasketDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly IProductService _productService;
    private readonly IBasketService _basketService;

    public CartController(IProductService productService, IBasketService basketService)
    {
        _productService = productService;
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

        var values = await _productService.GetByIdProductAsync(productId);
        if (values is null)
            return NotFound();

        var items = new BasketItemDto
        {
            ProductId = values.ProductId,
            ProductName = values.ProductName,
            ProductImageUrl = values.ProductImageUrl,
            ProductPrice = values.ProductPrice,
            Quantity = 1
        };
        await _basketService.AddBasketItemAsync(items);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> RemoveBasketItem(string productId)
    {
        await _basketService.RemoveBasketItemAsync(productId);
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateQuantity(string productId, int quantity)
    {
        if (string.IsNullOrWhiteSpace(productId) || quantity is < 1 or > 99)
            return BadRequest(new { success = false, message = "Ürün adedi 1 ile 99 arasında olmalıdır." });

        var basket = await _basketService.UpdateBasketItemQuantityAsync(productId, quantity);
        if (basket is null)
            return NotFound(new { success = false, message = "Sepetteki ürün bulunamadı." });

        var basketItem = basket.BasketItems.First(x => x.ProductId == productId);

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
