using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.ViewComponents.Home;

public class HomeVendorViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}