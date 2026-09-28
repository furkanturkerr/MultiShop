using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Models.Order;

namespace MultiShop.WebUI.ViewComponents.Order;

public class OrderDetailViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(OrderCheckoutViewModel model)
    {
        return View(model);
    }
}
