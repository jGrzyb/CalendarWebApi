using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using projekt.Data;
using projekt.Models;

namespace projekt.Controllers;

public class LoginController : Controller {
    private readonly IConfiguration _configuration;
    private readonly DataBaseContext _context;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public LoginController(IConfiguration configuration, DataBaseContext context, UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager) {
        _context = context;
        _configuration = configuration;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    [ActionName("Login")]
    public IActionResult Login() {
        return View();
    }

    [HttpPost]
    [ActionName("Login")]
    public async Task<IActionResult> Login(string username, string password) {
        var signInResult = await _signInManager.PasswordSignInAsync(username, password, false, false);

        if (!signInResult.Succeeded) return View();

        ViewData["Error"] = "Wrong username or password.";
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [ActionName("Register")]
    public IActionResult Register() {
        return View();
    }


    [HttpPost]
    [ActionName("Register")]
    public async Task<IActionResult> Register(string username, string email, string password) {
        Console.WriteLine(username + " " + email + " " + password);
        var identityUser = new IdentityUser {
            UserName = username,
            Email = email
        };

        var registerResult = await _userManager.CreateAsync(identityUser, password);

        if (!registerResult.Succeeded) {
            ViewData["Error"] = registerResult.Errors.FirstOrDefault()?.Description;
            return View();
        }

        var addRoleResult = await _userManager.AddToRoleAsync(identityUser, "User");
        var cId = 0;
        if (_context.Ownership.Any()) cId = _context.Ownership.Max(x => x.CalendarId) + 1;

        await _context.Ownership.AddAsync(new Ownership {
            UserId = Guid.Parse(identityUser.Id),
            CalendarId = cId
        });
        await _context.SaveChangesAsync();

        if (addRoleResult.Succeeded) return RedirectToAction("Login");

        //error
        ViewData["Error"] = registerResult.Errors.FirstOrDefault()?.Description;
        return View();
    }

    [HttpGet]
    [ActionName("Logout")]
    public async Task<IActionResult> Logout() {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }


    // private string GenerateJwtToken(string username)
    // {
    //     var tokenHandler = new JwtSecurityTokenHandler();
    //     var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);
    //     var tokenDescriptor = new SecurityTokenDescriptor
    //     {
    //         Subject = new ClaimsIdentity([
    //                 new Claim(ClaimTypes.Name, username)
    //             ]
    //         ),
    //         Expires = DateTime.UtcNow.AddMinutes(0.5),
    //         SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
    //         Issuer = _configuration["Jwt:Issuer"],
    //         Audience = _configuration["Jwt:Audience"]
    //     };
    //     var token = tokenHandler.CreateToken(tokenDescriptor);
    //     return tokenHandler.WriteToken(token);
    // }
}