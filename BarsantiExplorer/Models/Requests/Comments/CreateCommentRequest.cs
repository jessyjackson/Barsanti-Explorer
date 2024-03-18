using System.ComponentModel.DataAnnotations;

namespace BarsantiExplorer.Models.Requests.Comments
{
    public class CreateCommentRequest
    {
        [Required][MaxLength(30)] public string Title { get; set; }
        [Required][MaxLength(150)] public string Text { get; set; }
        [Required][Range(1,int.MaxValue)] public int TripId { get; set; }
    }
}
