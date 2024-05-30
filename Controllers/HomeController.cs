using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using projekt.Models;

namespace projekt.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [HttpGet, ActionName("Index")]
    public IActionResult Index()
    {
        var jwt = GetTokenIfValid();
        if (jwt == null)
        {
            return View();
        }

        ViewData["validTo"] = jwt.ValidTo;
        ViewData["currentDate"] = DateTime.UtcNow;
        return View();

        // return RedirectToAction("Login", "Login");
    }

    [HttpGet, ActionName("Privacy")]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private JwtSecurityToken? GetTokenIfValid()
    {
        var handler = Request.Cookies["jwt"];

        if (string.IsNullOrEmpty(handler))
        {
            return null;
        }

        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(handler);

        return jwt.ValidTo > DateTime.UtcNow
            ? jwt
            : null;
    }
}