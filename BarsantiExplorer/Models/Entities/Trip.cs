using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BarsantiExplorer.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace BarsantiExplorer.Models.Entities;

public class Trip : BaseEntity
{
    [MaxLength(50)] public string Title { get; set; }

    public string Description { get; set; }

    [MaxLength(256)] public string Image { get; set; }

    [MaxLength(100)] public string Address { get; set; }

    public double Latitude { get; set; }
    public double Longitude { get; set; }

    [MaxLength(12)] public string GeoHash { get; set; }

    //public int RatingsNumber { get; set; }
    //public double TripRating { get { return 0; } private set { } }

    [ForeignKey("TripType")] public int TypeId { get; set; }

    public virtual TripType TripType { get; set; }
    
    public TripResponse MapToTripResponse(IConfiguration appSettings, String host)
    {
        return new TripResponse
        {
            Id = this.Id,
            Title = this.Title,
            Description = this.Description,
            Image = $"{host}/{appSettings.GetValue<string>("UploadDir")}/{this.Image}",
            Address = this.Address,
            Latitude = this.Latitude,
            Longitude = this.Longitude,
            GeoHash = this.GeoHash,
            TripType = this.TripType,
            CreatedAt = this.CreatedAt,
            DeletedAt = this.DeletedAt
        };
    }
}