using BarsantiExplorer.Models;
using Microsoft.AspNetCore.Mvc;

namespace BarsantiExplorer.Controllers;

[ApiController]
[Route("api/trip-types")]
public class TripTypesController : BaseController
{
    public TripTypesController(BarsantiDbContext dbContext) : base(dbContext)
    {
    }
}