using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.ViewComponents.Home;

public class HomeOfferViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}