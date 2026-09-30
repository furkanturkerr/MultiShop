using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Models.Order;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class OrderController : Controller
{
    private readonly IOrderService _orderService;
    private readonly IOrderAddressService _addressService;

    public OrderController(IOrderService orderService, IOrderAddressService addressService)
    {
        _orderService = orderService;
        _addressService = addressService;
    }

    public async Task<IActionResult> Index()
    {
        var model = new AdminOrderListViewModel();
        try
        {
            model.Orders = (await _orderService.GetAllOrdersAsync())
                .OrderByDescending(x => x.OrderDate)
                .ThenByDescending(x => x.OrderingId)
                .ToList();
            model.Addresses = (await _addressService.GetAllOrderAddressesAsync())
                .ToDictionary(x => x.AddressId);
        }
        catch (HttpRequestException)
        {
            ViewData["LoadError"] = "Sipariş bilgileri alınamadı. Order servisi ve Gateway bağlantısını kontrol et.";
        }
        return View(model);
    }

    public async Task<IActionResult> Detail(int id)
    {
        if (id <= 0)
            return NotFound();

        try
        {
            var order = await _orderService.GetOrderDetailAsync(id);
            if (order is null)
                return NotFound();

            return View(new AdminOrderDetailViewModel
            {
                Order = order,
                Address = await _addressService.GetByIdOrderAddressAsync(order.AddressId)
            });
        }
        catch (HttpRequestException)
        {
            TempData["OrderError"] = "Sipariş detayı alınamadı. Lütfen tekrar deneyin.";
            return RedirectToAction(nameof(Index));
        }
    }
}
