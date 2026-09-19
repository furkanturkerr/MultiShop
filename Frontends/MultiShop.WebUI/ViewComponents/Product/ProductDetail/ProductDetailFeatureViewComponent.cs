using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.ViewComponents.Product.ProductDetail;

public class ProductDetailFeatureViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}