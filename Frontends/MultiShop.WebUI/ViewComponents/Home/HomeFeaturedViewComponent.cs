using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.ViewComponents.Home;

public class HomeFeaturedViewComponent : ViewComponent
{
    private readonly IFeaturedService _featuredService;

    public HomeFeaturedViewComponent(IFeaturedService featuredService)
    {
        _featuredService = featuredService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View(await _featuredService.GetActiveFeaturedAsync());
    }
}
