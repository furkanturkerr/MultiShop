using Microsoft.AspNetCore.Mvc;
using MultiShop.Dtos.CatalogDtos.ProductDtos;

namespace MultiShop.WebUI.ViewComponents.Product.ProductDetail;

public class ProductDetailFeatureViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(UpdateProductDto product)
    {
        return View(product);
    }
}
