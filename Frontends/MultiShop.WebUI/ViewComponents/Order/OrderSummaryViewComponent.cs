using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.ViewComponents.Order;

public class OrderSummaryViewComponent : ViewComponent
{
    private readonly IBasketService _basketService;

    public OrderSummaryViewComponent(IBasketService basketService)
    {
        _basketService = basketService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View(await _basketService.GetBasketAsync());
    }
}
