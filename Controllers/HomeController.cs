using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using imnobiliatiaNet.Models;

namespace imnobiliatiaNet.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    public IActionResult Panel()
    {
        var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
        if (usuarioId == null)
            return RedirectToAction("Login", "Auth");

        return View();
    }

}
