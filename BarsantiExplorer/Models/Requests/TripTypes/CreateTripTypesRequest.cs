using System.ComponentModel.DataAnnotations;

namespace BarsantiExplorer.Models.Requests.TripTypes
{
    public class CreateTripTypesRequest
    {
        [Required] public string Name { get; set; }
    }
}
