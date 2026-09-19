using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.ViewComponents.Product;

public class ProductListViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}