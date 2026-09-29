using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Dtos.CargoDtos.CargoCompanyDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[AutoValidateAntiforgeryToken]
[Authorize(Roles = "Admin,Manager")]
public class CargoCompanyController : Controller
{
    private readonly ICargoCompanyService _cargoCompanyService;

    public CargoCompanyController(ICargoCompanyService cargoCompanyService)
    {
        _cargoCompanyService = cargoCompanyService;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _cargoCompanyService.GetAllAsync());
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCargoCompanyDto dto)
    {
        dto.CompanyName = (dto.CompanyName ?? string.Empty).Trim();
        if (dto.CompanyName.Length < 2)
            ModelState.AddModelError(nameof(dto.CompanyName), "Şirket adı en az 2 karakter olmalıdır.");

        if (!ModelState.IsValid)
            return View(dto);

        if (!await _cargoCompanyService.CreateAsync(dto))
        {
            ModelState.AddModelError(string.Empty, "Kargo şirketi kaydedilemedi.");
            return View(dto);
        }

        TempData["CargoCompanySuccess"] = "Kargo şirketi oluşturuldu.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var company = await _cargoCompanyService.GetByIdAsync(id);
        return company is null ? NotFound() : View(company);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateCargoCompanyDto dto)
    {
        dto.CompanyName = (dto.CompanyName ?? string.Empty).Trim();
        if (dto.CompanyName.Length < 2)
            ModelState.AddModelError(nameof(dto.CompanyName), "Şirket adı en az 2 karakter olmalıdır.");

        if (!ModelState.IsValid)
            return View(dto);

        if (!await _cargoCompanyService.UpdateAsync(dto))
        {
            ModelState.AddModelError(string.Empty, "Kargo şirketi güncellenemedi.");
            return View(dto);
        }

        TempData["CargoCompanySuccess"] = "Kargo şirketi güncellendi.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _cargoCompanyService.DeleteAsync(id);
        TempData[deleted ? "CargoCompanySuccess" : "CargoCompanyError"] =
            deleted ? "Kargo şirketi silindi." : "Kargo şirketi silinemedi.";

        return RedirectToAction(nameof(Index));
    }
}
