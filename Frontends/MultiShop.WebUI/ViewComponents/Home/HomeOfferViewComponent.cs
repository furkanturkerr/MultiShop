using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.ViewComponents.Home;

public class HomeOfferViewComponent : ViewComponent
{
    private readonly IOfferDiscountService _offerDiscountService;

    public HomeOfferViewComponent(IOfferDiscountService offerDiscountService)
    {
        _offerDiscountService = offerDiscountService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View(await _offerDiscountService.GetActiveOfferDiscountAsync());
    }
}
