using System.ComponentModel.DataAnnotations.Schema;

namespace BarsantiExplorer.Models.Entities;

public class Trip
{
    public String Title { get; set; }
    public String Description { get; set; }
    public String Image { get; set; }
    
    [ForeignKey("TripType")] 
    public int TypeId { get; set; }

    public virtual TripType TripType { get; set; }
}