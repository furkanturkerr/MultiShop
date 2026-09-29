using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Manager")]
public class CargoCustomerController : Controller
{
    private readonly ICargoCustomerService _cargoCustomerService;

    public CargoCustomerController(ICargoCustomerService cargoCustomerService)
    {
        _cargoCustomerService = cargoCustomerService;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _cargoCustomerService.GetAllAsync());
    }
}
