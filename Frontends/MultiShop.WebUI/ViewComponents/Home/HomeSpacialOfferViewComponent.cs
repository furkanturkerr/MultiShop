using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.ViewComponents.Home;

public class HomeSpacialOfferViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}