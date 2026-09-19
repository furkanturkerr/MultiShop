using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.ViewComponents.Product.ProductDetail;

public class ProductDetailImagesViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}