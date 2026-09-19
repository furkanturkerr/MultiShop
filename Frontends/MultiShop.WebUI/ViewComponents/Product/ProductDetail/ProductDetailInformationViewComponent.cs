using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.ViewComponents.Product.ProductDetail;

public class ProductDetailInformationViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}