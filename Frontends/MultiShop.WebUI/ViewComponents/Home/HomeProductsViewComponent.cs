using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.ViewComponents.Home;

public class HomeProductsViewComponent : ViewComponent
{
    private readonly IProductService _productService;

    public HomeProductsViewComponent(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View(await _productService.GetAllProductAsync());
    }
}
