using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.ViewComponents.Product.ProductDetail;

public class ProductDetailReviewViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}