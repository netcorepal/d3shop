using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace NetCorePal.D3Shop.Web.AuthenticationServer.Controllers;

public class HomeController(ILogger<HomeController> logger) : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Error()
    {
        return View();
    }
}
