using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.ViewComponents.Product;

public class ProductSizeFilterViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}