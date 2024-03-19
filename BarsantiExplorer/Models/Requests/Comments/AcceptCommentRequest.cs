using System.ComponentModel.DataAnnotations;

namespace BarsantiExplorer.Models.Requests.Comments
{
    public class AcceptCommentRequest
    {
        [Required] public bool IsAccepted { get; set; }
    }
}
