using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.ViewComponents.Home;

public class HomeCarouselViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}