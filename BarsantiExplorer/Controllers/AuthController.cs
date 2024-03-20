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
    /// <summary>
    /// Login
    /// </summary>
    /// <response code="200">Returns the jwt Token</response> 
    /// <responde code="400">If the username or password are invalid</response>
    [HttpPost("")]
    public IActionResult GenerateToken([FromForm] AuthTokenRequest authTokenRequest)
    {
        var user = DB.Users.FirstOrDefault(u => u.Email == authTokenRequest.Email);
        if (user == null)
        {
            return BadRequest("Invalid Username");
        }
        if (user.Password != authTokenRequest.Password)
        {
            return BadRequest("Invalid Password");
        }
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Email,authTokenRequest.Email),
            new(JwtRegisteredClaimNames.Sub,authTokenRequest.Email),
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
        var jwt = tokenHandler.WriteToken(token);
        return Ok(jwt);
    }

}