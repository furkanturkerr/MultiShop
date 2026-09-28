using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.ViewComponents.Product.ProductDetail;

public class ProductDetailInformationViewComponent : ViewComponent
{
    private readonly IProductDetailService _productDetailService;

    public ProductDetailInformationViewComponent(IProductDetailService productDetailService)
    {
        _productDetailService = productDetailService;
    }

    public async Task<IViewComponentResult> InvokeAsync(string id)
    {
        return View(await _productDetailService.GetResultProductDetailByProductIdAsync(id));
    }
}
