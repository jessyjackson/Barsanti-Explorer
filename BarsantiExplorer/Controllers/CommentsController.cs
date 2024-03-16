using BarsantiExplorer.Models;
using Microsoft.AspNetCore.Mvc;

namespace BarsantiExplorer.Controllers;

[ApiController]
[Route("api/comments")]
public class CommentsController: BaseController
{
    public CommentsController(BarsantiDbContext context) : base(context)
    {
    }
}