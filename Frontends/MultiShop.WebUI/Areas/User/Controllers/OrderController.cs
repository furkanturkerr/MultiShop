using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Areas.User.Controllers;

[Area("User")]
[Authorize]
public class OrderController : Controller
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    // GET
    public async Task<IActionResult> OrderHistory()
    {
        var values = await _orderService.GetMyOrdersAsync();
        return View(values);
    }
}
