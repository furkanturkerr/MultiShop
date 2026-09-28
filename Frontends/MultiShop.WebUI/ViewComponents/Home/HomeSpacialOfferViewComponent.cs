using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.ViewComponents.Home;

public class HomeSpacialOfferViewComponent : ViewComponent
{
    private readonly ISpecialOfferService _specialOfferService;

    public HomeSpacialOfferViewComponent(ISpecialOfferService specialOfferService)
    {
        _specialOfferService = specialOfferService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View(await _specialOfferService.GetActiveSpecialOfferAsync());
    }
}
