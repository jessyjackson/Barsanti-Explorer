using BarsantiExplorer.Models.Entities;

namespace BarsantiExplorer.Models.Responses
{
    public class CommentResponse
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public Trip Trip { get; set; }
    }
}
