using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.ViewComponents.Home;

public class HomeCategoriesViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}