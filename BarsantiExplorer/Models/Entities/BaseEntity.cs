using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarsantiExplorer.Models.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? DeletedAt { get; set; }
    }
}
