using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using projekt.Models;

namespace projekt.Controllers;
[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index() {
        var jwt = getTokenIfValid();
        if(jwt != null) {
            ViewData["validTo"] = jwt.ValidTo;
            ViewData["currentDate"] = DateTime.UtcNow;
            return View();
        }
        // return RedirectToAction("Login", "Login");
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

    private JwtSecurityToken? getTokenIfValid() {
        var handler = Request.Cookies["jwt"];
        if(!string.IsNullOrEmpty(handler)) {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(handler);
            if(jwt.ValidTo > DateTime.UtcNow) {
                return jwt;
            }
        }
        return null;
    }
}
