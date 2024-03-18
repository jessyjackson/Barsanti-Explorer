namespace BarsantiExplorer.Models.Requests.Trips;

public class GetTripsRequest
{
    public int? TripType { get; set; }
    public string? Search { get; set; }
    public string? Sort { get; set; }
    public string? Order { get; set; }
    public int? Page { get; set; }
    public int? Limit { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public int? GeoHashPrecision { get; set; }
    public bool? IncludeDeleted { get; set; }
}