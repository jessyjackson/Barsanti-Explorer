using BarsantiExplorer.Models;
using BarsantiExplorer.Models.Entities;
using BarsantiExplorer.Models.Requests;
using BarsantiExplorer.Models.Responses;
using Geohash;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarsantiExplorer.Controllers;

[ApiController]
[Route("api/trips")]
public class TripsController : BaseController
{
    Geohasher geoHasher = new Geohasher();

    public TripsController(BarsantiDbContext context, IConfiguration appSettings) : base(context, appSettings)
    {
    }

    /// <summary>
    ///  Get filtered trips
    /// </summary>
    /// <response code="200">Returns filtered trips</response>
    [HttpGet("")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<Trip>), StatusCodes.Status200OK)]
    public IActionResult GetTrips()
    {
        var con = AppSettings.GetConnectionString("DefaultConnection");
        var trips = DB.Trips.Include(el => el.TripType).ToList();

        var domain = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host.Value;
        return Ok(trips.Select(el => el.MapToTripResponse(AppSettings, domain)));
    }

    /// <summary>
    /// Create a new trip
    /// </summary>
    /// <response code="200">Returns the new trips data</response>
    [HttpPost("")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(TripResponse), StatusCodes.Status200OK)]
    public IActionResult CreateTrip([FromForm] CreateTripRequest body)
    {
        string uploadsFolder =
            Path.Combine(Directory.GetCurrentDirectory(), AppSettings.GetValue<string>("UploadDir"));

        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        string uniqueFileName = Path.GetRandomFileName() + Path.GetExtension(body.Image.FileName);
        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

        // Save the uploaded image
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            body.Image.CopyTo(fileStream);
        }

        // calculate geo hash
        var geoHash = geoHasher.Encode(body.Latitude, body.Longitude, 6);

        var trip = new Trip
        {
            Title = body.Title,
            Description = body.Description,
            Address = body.Address,
            Latitude = body.Latitude,
            Longitude = body.Longitude,
            TypeId = body.TypeId,
            Image = uniqueFileName,
            GeoHash = geoHash,
        };

        DB.Trips.Add(trip);
        DB.SaveChanges();

        var domain = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host.Value;
        return Ok(trip.MapToTripResponse(AppSettings, domain));
    }
    
    /// <summary>
    /// update a trip
    /// </summary>
    /// <response code="200">Returns the updated trips data</response>
    [HttpPut("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(TripResponse), StatusCodes.Status200OK)]
    public IActionResult UpdateTrip(int id, [FromForm] UpdateTripRequest body)
    {
        var trip = DB.Trips.Find(id);
        if (trip == null)
        {
            return NotFound();
        }

        if (body.Image != null)
        {
            string uploadsFolder =
                Path.Combine(Directory.GetCurrentDirectory(), AppSettings.GetValue<string>("UploadDir"));

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string uniqueFileName = Path.GetRandomFileName() + Path.GetExtension(body.Image.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save the uploaded image
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                body.Image.CopyTo(fileStream);
            }

            trip.Image = uniqueFileName;
        }

        if(body.Latitude != null) trip.Latitude = body.Latitude.Value;
        if(body.Longitude != null) trip.Longitude = body.Longitude.Value;
        if(body.Title != null) trip.Title = body.Title;
        if(body.Description != null) trip.Description = body.Description;
        if(body.Address != null) trip.Address = body.Address;
        if(body.TypeId != null) trip.TypeId = body.TypeId.Value;
        
        var geoHash = geoHasher.Encode(trip.Latitude, trip.Longitude, 6);
        trip.GeoHash = geoHash;

        DB.SaveChanges();

        var domain = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host.Value;
        return Ok(trip.MapToTripResponse(AppSettings, domain));
    }
    
    /// <summary>
    /// Delete a trip
    /// </summary>
    /// <response code="200">Returns true if the trip was deleted successfully</response>
    [HttpDelete("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public IActionResult DeleteTrip(int id)
    {
        var trip = DB.Trips.Find(id);
        if (trip == null)
        {
            return NotFound();
        }

        trip.DeletedAt = DateTime.Now;
        DB.SaveChanges();

        return Ok(true);
    }
}