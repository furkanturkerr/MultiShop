using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.ViewComponents.Cart;

public class CartProductListViewComponent : ViewComponent
{
    private readonly IBasketService _basketService;

    public CartProductListViewComponent(IBasketService basketService)
    {
        _basketService = basketService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var basketTotal = await _basketService.GetBasketAsync();
        return View(basketTotal);
    }
}
