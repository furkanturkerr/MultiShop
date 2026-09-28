using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.ViewComponents.Product.ProductDetail;

public class ProductDetailFeatureViewComponent : ViewComponent
{
    private readonly IProductService _productService;

    public ProductDetailFeatureViewComponent(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IViewComponentResult> InvokeAsync(string id)
    {
        return View(await _productService.GetByIdProductAsync(id));
    }
}
