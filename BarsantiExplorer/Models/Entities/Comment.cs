using BarsantiExplorer.Models.Responses;
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
        public bool IsAccepted { get; set; }

        [ForeignKey("Trip")]
        public int TripId { get; set; }
        public virtual Trip Trip { get; set; }
        public CommentResponse CommentToCommentResponse()
        {
            return new CommentResponse
            {
                Title = this.Title,
                Text = this.Text,
            };
        }
    }
}
