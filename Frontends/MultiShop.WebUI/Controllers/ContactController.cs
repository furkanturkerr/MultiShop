using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.Controllers;

public class ContactController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}