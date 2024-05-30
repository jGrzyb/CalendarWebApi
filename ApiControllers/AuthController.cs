using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using projekt.Data;
using projekt.Models;

namespace projekt.ApiControllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly DataBaseContext _context;

    public AuthController(IConfiguration configuration, DataBaseContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<IEnumerable<string>>> Login(User user)
    {
        var us = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email && u.Password == user.Password);
        if (us == null)
        {
            return BadRequest();
        }
        
        var token = GenerateJwtToken(user.Email);
        Response.Cookies.Append("jwt", token);
        return new List<string> { token };

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
}