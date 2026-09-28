using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.ViewComponents.Home;

public class HomeCarouselViewComponent : ViewComponent
{
    private readonly IFeatureSliderService _featureSliderService;

    public HomeCarouselViewComponent(IFeatureSliderService featureSliderService)
    {
        _featureSliderService = featureSliderService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View(await _featureSliderService.GetActiveFeatureSliderAsync());
    }
}
