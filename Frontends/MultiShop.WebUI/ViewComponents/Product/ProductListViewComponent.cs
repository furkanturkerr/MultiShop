using Microsoft.AspNetCore.Mvc;
using MultiShop.Dtos.CatalogDtos.ProductDtos;

namespace MultiShop.WebUI.ViewComponents.Product;

public class ProductListViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(List<ResultProductDto> products)
    {
        return View(products);
    }
}
