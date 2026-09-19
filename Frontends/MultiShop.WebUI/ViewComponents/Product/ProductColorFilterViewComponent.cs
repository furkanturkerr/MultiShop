using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.ViewComponents.Product;

public class ProductColorFilterViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}