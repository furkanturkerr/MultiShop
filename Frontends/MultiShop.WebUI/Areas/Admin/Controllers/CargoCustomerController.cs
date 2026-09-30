using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Dtos.OrderDtos.OrderAddressDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Manager")]
public class CargoCustomerController : Controller
{
    private readonly IOrderAddressService _addressService;

    public CargoCustomerController(IOrderAddressService addressService)
    {
        _addressService = addressService;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var addresses = await _addressService.GetAllOrderAddressesAsync();
            return View(addresses.OrderByDescending(x => x.AddressId).ToList());
        }
        catch (HttpRequestException)
        {
            ViewData["LoadError"] = "Müşteri adresleri alınamadı. Order servisi ve Gateway bağlantısını kontrol et.";
            return View(new List<ResultOrderAddressDto>());
        }
    }
}
