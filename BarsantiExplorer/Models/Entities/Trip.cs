using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BarsantiExplorer.Models.Entities;

public class Trip : BaseEntity
{
    [MaxLength(50)]
    public string Title { get; set; }
    
    public string Description { get; set; }
    
    [MaxLength(256)]
    public string Image { get; set; }
    
    [MaxLength(100)]
    public string Address { get; set; }
    
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    
    [MaxLength(12)]
    public string GeoHash { get; set; }
    
    [ForeignKey("TripType")] 
    public int TypeId { get; set; }

    public virtual TripType TripType { get; set; }
}
