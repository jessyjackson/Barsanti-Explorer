using BarsantiExplorer.Models;
using BarsantiExplorer.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarsantiExplorer.Controllers;

[ApiController]
[Route("api/trips")]
public class TripsController : BaseController
{
    public TripsController(BarsantiDbContext context) : base(context)
    {
    }

    /// <summary>
    ///  Get all trips
    /// </summary>
    /// <response code="200">Returns all tripss</response>
    [HttpGet("")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<Trip>), StatusCodes.Status200OK)]
    public IActionResult GetTrips()
    {
        var trips = DB.Trips.Include(el => el.TripType).ToList();
        return Ok(trips);
    }
}