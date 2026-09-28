using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.ViewComponents.Home;

public class HomeVendorViewComponent : ViewComponent
{
    private readonly IBrandService _brandService;

    public HomeVendorViewComponent(IBrandService brandService)
    {
        _brandService = brandService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View(await _brandService.GetActiveBrandAsync());
    }
}
