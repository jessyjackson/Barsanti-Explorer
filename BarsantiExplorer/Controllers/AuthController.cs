using BarsantiExplorer.Models;
using Microsoft.AspNetCore.Mvc;

namespace BarsantiExplorer.Controllers;

[ApiController]
[Route("auth")]
public class AuthController: BaseController
{
    public AuthController(BarsantiDbContext context): base(context) { }
}