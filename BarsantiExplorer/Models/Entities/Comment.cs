using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarsantiExplorer.Models.Entities
{
    public class Comment: BaseEntity
    {
        [MaxLength(30)]
        public string Title { get; set; }
        
        [MaxLength(150)]
        public string Text { get; set; }
        
        [MaxLength(256)]
        public string Image { get; set; }

        [ForeignKey("Trip")]
        public int TripId { get; set; }
        public virtual Trip Trip { get; set; }
    }
}
