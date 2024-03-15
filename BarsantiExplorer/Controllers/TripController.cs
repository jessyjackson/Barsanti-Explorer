using Microsoft.AspNetCore.Mvc;

namespace BarsantiExplorer.Controllers;

[ApiController]
[Route("trips")]
public class TripController
{
    /// <summary>
    ///  Get all trips
    /// </summary>
    /// <response code="200">Returns all tripss</response>
    [HttpGet("")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(String), StatusCodes.Status200OK)]
    public async Task<String> GetTrips()
    {
        return "trips";
    }
}