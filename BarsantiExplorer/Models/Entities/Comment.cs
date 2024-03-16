using System.ComponentModel.DataAnnotations.Schema;

namespace BarsantiExplorer.Models.Entities
{
    public class Comment: BaseEntity
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string Image { get; set; }

        [ForeignKey("Trip")]
        public int TripId { get; set; }
        public virtual Trip Trip { get; set; }
    }
}
