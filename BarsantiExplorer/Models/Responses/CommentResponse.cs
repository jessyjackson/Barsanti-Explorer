using BarsantiExplorer.Models.Entities;

namespace BarsantiExplorer.Models.Responses
{
    public class CommentResponse
    {
        public string Author { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }

        public Trip Trip { get; set; }
    }
}