using System.ComponentModel.DataAnnotations;

namespace BarsantiExplorer.Models.Entities;

public class TripType: BaseEntity
{
    [MaxLength(50)]
    public String Name { get; set; }
}