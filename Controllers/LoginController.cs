using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using projekt.Data;
using projekt.Models;

namespace projekt.Controllers;

public class LoginController : Controller
{
    private readonly DataBaseContext _context;
    private readonly IConfiguration _configuration;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public LoginController(IConfiguration configuration, DataBaseContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _context = context;
        _configuration = configuration;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet, ActionName("Login")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost, ActionName("Login")]
    public async Task<IActionResult> Login(User user)
    {
        var signInResult = await _signInManager.PasswordSignInAsync("admin@gmail.com", "admin", false, false);

        if (!signInResult.Succeeded)
        {
            return View();
        }

        return RedirectToAction("Index", "Home");
    }


    [HttpPost, ActionName("Register")]
    public async Task<IActionResult> Register(string username, string email, string password)
    {
        var identityUser = new IdentityUser
        {
            UserName = username,
            Email = email
        };

        var registerResult = await _userManager.CreateAsync(identityUser, password);

        if (!registerResult.Succeeded)
        {
            return View();
        }

        var addRoleResult = await _userManager.AddToRoleAsync(identityUser, "User");

        if (addRoleResult.Succeeded)
        {
            return RedirectToAction("Login");
        }

        //error
        return View();
    }


    private string GenerateJwtToken(string username)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                    new Claim(ClaimTypes.Name, username)
                ]
            ),
            Expires = DateTime.UtcNow.AddMinutes(0.5),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"]
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    [HttpGet, ActionName("Logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}