using System.ComponentModel.DataAnnotations;

namespace BarsantiExplorer.Models.Requests.Comments
{
    public class CreateCommentRequest
    {
        [MaxLength(50)] [Required] public string Author { get; set; }
        [MaxLength(150)] [Required] public string Text { get; set; }
        [Range(1, 5)] [Required] public int Rating { get; set; }
        [Range(1, int.MaxValue)] public int TripId { get; set; }
    }
}