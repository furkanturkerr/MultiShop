using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.Controllers;

public class ProductController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Detail()
    {
        return View();
    }
}