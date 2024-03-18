using System.ComponentModel.DataAnnotations;

namespace BarsantiExplorer.Models.Requests.Comments
{
    public class UpdateCommentRequest
    {
        [MaxLength(30)] public string? Title { get; set; }
        [MaxLength(150)] public string? Text { get; set; }
        [Range(1, int.MaxValue)] public int TripId { get; set; }
    }
}
