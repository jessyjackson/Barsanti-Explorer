using System.ComponentModel.DataAnnotations;
using BarsantiExplorer.Models.Entities;

namespace BarsantiExplorer.Models.Responses;

public class TripResponse: BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public string Address { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string GeoHash { get; set; }
    public TripType TripType { get; set; }
    public double AvarageRating { get; set; }
}