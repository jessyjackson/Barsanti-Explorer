using System.ComponentModel.DataAnnotations;

namespace BarsantiExplorer.Models.Requests.Comments
{
    public class CreateCommentRequest
    {
        [MaxLength(30)][Required] public string Title { get; set; }
        [MaxLength(150)][Required] public string Text { get; set; }
        [Range(1,int.MaxValue)] public int TripId { get; set; }
    }
}
