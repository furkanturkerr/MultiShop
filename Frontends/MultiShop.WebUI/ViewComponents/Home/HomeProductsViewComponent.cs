using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.ViewComponents.Home;

public class HomeProductsViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}