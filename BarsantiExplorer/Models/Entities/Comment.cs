using BarsantiExplorer.Enum;
using BarsantiExplorer.Models.Responses;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarsantiExplorer.Models.Entities
{
    public class Comment: BaseEntity
    {
        [MaxLength(50)]
        public string Author { get; set; }
        
        [MaxLength(150)]
        public string Text { get; set; }
        public int Rating { get; set; }
        public CommentStatus Status { get; set; }

        [ForeignKey("Trip")]
        public int TripId { get; set; }
        public virtual Trip Trip { get; set; }
        public CommentResponse MapToCommentResponse()
        {
            return new CommentResponse
            {
                Author = this.Author,
                Text = this.Text,
                Rating = this.Rating
            };
        }
    }
}
