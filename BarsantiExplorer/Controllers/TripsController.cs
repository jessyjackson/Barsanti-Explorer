using BarsantiExplorer.Models;
using BarsantiExplorer.Models.Entities;
using BarsantiExplorer.Models.Requests.Trips;
using BarsantiExplorer.Models.Responses;
using Geohash;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic;
using System.Linq.Dynamic.Core;

namespace BarsantiExplorer.Controllers;

[ApiController]
[Route("api/trips")]
public class TripsController : BaseController
{
    private Geohasher _geoHasher = new();

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
    public IActionResult GetTrips([FromQuery] GetTripsRequest queryParams)
    {
        var trips = DB.Trips
            .Include(el => el.TripType)
            .AsQueryable();

        if (queryParams.IncludeDeleted == null)
        {
            trips = trips.Where(el => el.DeletedAt == null);
        }

        if (queryParams.TripType != null)
        {
            trips = trips.Where(el => el.TypeId == queryParams.TripType);
        }

        if (queryParams.Search != null)
        {
            trips = trips.Where(el =>
                el.Title.Contains(queryParams.Search) || el.Description.Contains(queryParams.Search));
        }
        if (queryParams.Latitude != null && queryParams.Longitude != null)
        {
            var geoHashPrecision = queryParams.GeoHashPrecision ?? 4;
            string geoHash =
                _geoHasher.Encode(queryParams.Latitude.Value, queryParams.Longitude.Value, geoHashPrecision);
            var neighbours = _geoHasher.GetNeighbors(geoHash);

            List<string> zoneHashes = neighbours.Values
                .Concat(new List<string> { geoHash })
                .ToList();

            trips = trips.Where(el => zoneHashes.Contains(el.GeoHash.Substring(0, geoHashPrecision)));
        }

        if (queryParams.Sort != null)
        {
            var order = queryParams.Order ?? "desc";
            trips = trips.OrderBy(queryParams.Sort + " " + order);
        }

        int pageItems = 20;
        if (queryParams.Limit != null)
        {
            pageItems = queryParams.Limit.Value;
        }

        if (queryParams.Page != null)
        {
            trips = trips.Skip(queryParams.Page.Value * pageItems).Take(pageItems);
        }

        var domain = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host.Value;
        return Ok(trips.Select(el => el.MapToTripResponse(AppSettings, domain)));
    }

    /// <summary>
    /// Get a trip
    /// </summary>
    /// <response code="200">Returns the trip data</response>
    /// <response code="404">If the trip was not found</response>
    [HttpGet("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(TripResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetTrip(int id)
    {
        var trip = DB.Trips
            .Include(el => el.TripType)
            .Where(el => el.DeletedAt == null)
            .FirstOrDefault(el => el.Id == id);

        if (trip == null)
        {
            return NotFound();
        }

        var domain = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host.Value;
        return Ok(trip.MapToTripResponse(AppSettings, domain));
    }

    /// <summary>
    /// Create a new trip
    /// </summary>
    /// <response code="200">Returns the new trips data</response>  
    [Authorize]
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
        double latitude = double.Parse(body.Latitude);
        double longitude = double.Parse(body.Longitude);
        var geoHash = _geoHasher.Encode(latitude, longitude, 6);

        var trip = new Trip
        {
            Title = body.Title,
            Description = body.Description,
            Address = body.Address,
            Latitude = latitude,
            Longitude = longitude,
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
    [Authorize]
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

        if (body.Latitude != null) trip.Latitude = double.Parse(body.Latitude.Replace(".", ","));
        if (body.Longitude != null) trip.Longitude = double.Parse(body.Longitude.Replace(".", ","));
        if (body.Title != null) trip.Title = body.Title;
        if (body.Description != null) trip.Description = body.Description;
        if (body.Address != null) trip.Address = body.Address;
        if (body.TypeId != null) trip.TypeId = body.TypeId.Value;

        var geoHash = _geoHasher.Encode(trip.Latitude, trip.Longitude, 6);
        trip.GeoHash = geoHash;

        DB.SaveChanges();

        var domain = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host.Value;
        return Ok(trip.MapToTripResponse(AppSettings, domain));
    }

    /// <summary>
    /// Delete a trip
    /// </summary>
    /// <response code="200">Returns true if the trip was deleted successfully</response>
    [Authorize]
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