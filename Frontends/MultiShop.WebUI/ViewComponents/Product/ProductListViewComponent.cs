using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.ViewComponents.Product;

public class ProductListViewComponent : ViewComponent
{
    private readonly IProductService _productService;

    public ProductListViewComponent(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IViewComponentResult> InvokeAsync(string categoryId)
    {
        return View(await _productService.GetProductByCategoryIdAsync(categoryId));
    }
}
