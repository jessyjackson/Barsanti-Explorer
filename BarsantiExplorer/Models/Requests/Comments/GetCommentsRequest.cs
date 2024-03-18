namespace BarsantiExplorer.Models.Requests.Comments
{
    public class GetCommentsRequest
    {
        public string? Text { get; set; }
        public string? Title { get; set; }
        public int? TripId { get; set; }
    }

}
