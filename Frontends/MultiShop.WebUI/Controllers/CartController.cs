using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.Controllers;

public class CartController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}