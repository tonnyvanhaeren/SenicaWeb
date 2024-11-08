using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SenicaWeb.Models;

namespace SenicaWeb.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<AppUser> _userMgr;

    public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userMgr)
    {
        _logger = logger;
        _userMgr = userMgr;
    }

    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> Secured()
    {
        AppUser user = await _userMgr.GetUserAsync(HttpContext.User);
        string message = "Hello " + user.Voornaam + " " + user.Familienaam;
        return View((object)message);
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

    [Route("/Home/HandleError/{code:int}")]
    public IActionResult HandleError(int code)
    {
        ViewData["ErrorMessage"] = $"Er is iets verkeerd gebeurd CODE -> {code}";
        return View("~/Views/Shared/HandleError.cshtml");
    }
}
