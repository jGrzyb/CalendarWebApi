using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using projekt.Models;

namespace projekt.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet, ActionName("Index")]
    public IActionResult Index()
    {
        ViewData["logged"] = "Not logged in";
        if (_signInManager.IsSignedIn(User))
        {
            ViewData["logged"] = "Logged in";
            ViewData["id"] = _userManager.GetUserId(User);
            ViewData["name"] = _userManager.GetUserName(User);
        }
        
        
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

    // private JwtSecurityToken? GetTokenIfValid()
    // {
    //     var handler = Request.Cookies["jwt"];
    //
    //     if (string.IsNullOrEmpty(handler))
    //     {
    //         return null;
    //     }
    //
    //     var jwt = new JwtSecurityTokenHandler().ReadJwtToken(handler);
    //
    //     return jwt.ValidTo > DateTime.UtcNow
    //         ? jwt
    //         : null;
    // }
}