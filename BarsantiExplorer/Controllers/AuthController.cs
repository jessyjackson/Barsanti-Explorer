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
using Microsoft.AspNetCore.Authorization;


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
    /// <response code="401">If the username or password are invalid</response>
    [ProducesResponseType(typeof(LoginResponse),200)]
    [ProducesResponseType(401)]
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest body)
    {
        var user = DB.Users.FirstOrDefault(u => u.Email == body.Email);
        if (user == null || user.Password != body.Password)
        {
            return Unauthorized();
        }
        
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Email,body.Email),
            new(JwtRegisteredClaimNames.Sub,body.Email),
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
        
        var response = new LoginResponse() 
        {
            User = user,
            Token = jwt
        };
        
        return Ok(response);
    }


    /// <summary>
    /// Get current user
    /// </summary>
    /// <response code="200">Returns the current user</response>
    /// <response code="401">If the user is not authenticated</response>
    [ProducesResponseType(typeof(User), 200)]
    [ProducesResponseType(401)]
    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        var email = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value;
        if (email == null)
        {
            return Unauthorized();
        }

        var user = DB.Users.FirstOrDefault(u => u.Email == email);
        if (user == null)
        {
            return Unauthorized();
        }

        return Ok(user);
    }
}