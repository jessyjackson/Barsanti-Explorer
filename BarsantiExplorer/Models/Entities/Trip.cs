using System.ComponentModel.DataAnnotations.Schema;

namespace BarsantiExplorer.Models.Entities;

public class Trip : BaseEntity
{
    public String Title { get; set; }
    public String Description { get; set; }
    public String Image { get; set; }
    public String Address { get; set; }
    public String Latitude { get; set; }
    public String Longitude { get; set; }
    public String GeoHash { get; set; }
    
    [ForeignKey("TripType")] 
    public int TypeId { get; set; }

    public virtual TripType TripType { get; set; }
}
