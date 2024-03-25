using BarsantiExplorer.Enum;
using System.ComponentModel.DataAnnotations;

namespace BarsantiExplorer.Models.Requests.Comments
{
    public class AcceptCommentRequest
    {
        [Required] public CommentStatus Status  { get; set; }
    }
}
