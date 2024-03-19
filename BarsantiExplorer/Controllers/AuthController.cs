using BarsantiExplorer.Models;
using BarsantiExplorer.Models.Entities;
using BarsantiExplorer.Models.Requests.Auth;
using BarsantiExplorer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;


namespace BarsantiExplorer.Controllers;

[ApiController]
[Route("auth")]
public class AuthController: BaseController
{
    private JwtOptions JwtOptions { get; set; }
    public AuthController(BarsantiDbContext context, IConfiguration appSettings) : base(context,appSettings)
    {
       JwtOptions = appSettings.GetSection("JwtOptions").Get<JwtOptions>()!;
    }

    [HttpPost("")]
    public IActionResult GenerateToken([FromForm] AuthTokenRequest authTokenRequest)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, authTokenRequest.Email)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.SigningKey));
        var tokenExpirationHours = Convert.ToInt32(JwtOptions.ExpirationHours);

        var tokenDescription = new SecurityTokenDescriptor
        {
            Issuer = JwtOptions.Issuer,
            Audience = JwtOptions.Audience,
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(tokenExpirationHours),
            SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescription);
        return Ok(token);
    }

}