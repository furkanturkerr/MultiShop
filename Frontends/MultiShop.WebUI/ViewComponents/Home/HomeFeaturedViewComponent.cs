using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.ViewComponents.Home;

public class HomeFeaturedViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}