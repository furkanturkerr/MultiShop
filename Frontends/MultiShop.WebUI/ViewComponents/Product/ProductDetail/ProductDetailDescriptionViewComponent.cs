using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.ViewComponents.Product.ProductDetail;

public class ProductDetailDescriptionViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}