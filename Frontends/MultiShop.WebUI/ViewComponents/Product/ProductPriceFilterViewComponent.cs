using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.ViewComponents.Product;

public class ProductPriceFilterViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}