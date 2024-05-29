using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace projekt.Controllers {
    [Route("login")]
    public class LoginController : Controller {
        private readonly DataBaseContext _context;
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration, DataBaseContext context) {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Login() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User user) {
            var us = await _context.User.FirstOrDefaultAsync(u => u.Email == user.Email && u.Password == user.Password);
            if(us != null) {
                Response.Cookies.Append("jwt", GenerateJwtToken(user.Email));
                return RedirectToAction("Index", "Home");
            }
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
                ]),
                Expires = DateTime.UtcNow.AddMinutes(0.5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}