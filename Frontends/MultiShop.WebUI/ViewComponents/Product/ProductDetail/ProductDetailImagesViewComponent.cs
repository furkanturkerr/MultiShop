using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.ViewComponents.Product.ProductDetail;

public class ProductDetailImagesViewComponent : ViewComponent
{
    private readonly IProductImageService _productImageService;

    public ProductDetailImagesViewComponent(IProductImageService productImageService)
    {
        _productImageService = productImageService;
    }

    public async Task<IViewComponentResult> InvokeAsync(string productId)
    {
        return View(await _productImageService.GetProductImageByProductIdAsync(productId));
    }
}
