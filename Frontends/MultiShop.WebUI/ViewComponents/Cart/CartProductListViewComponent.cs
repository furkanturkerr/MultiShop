using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.ViewComponents.Cart;

public class CartProductListViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}