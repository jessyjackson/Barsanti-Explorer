using System.ComponentModel.DataAnnotations;

namespace BarsantiExplorer.Models.Requests.Trips;

public class CreateTripRequest
{
    [Required] public IFormFile Image { get; set; }

    [MaxLength(50)][Required] public string Title { get; set; }

    [Required] public string Description { get; set; }

    [MaxLength(100)][Required] public string Address { get; set; }

    [Required] public double Latitude { get; set; }

    [Required] public double Longitude { get; set; }

    [Range(1, int.MaxValue)] public int TypeId { get; set; }
}