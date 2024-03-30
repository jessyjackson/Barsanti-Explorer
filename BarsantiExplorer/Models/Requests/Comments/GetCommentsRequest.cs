namespace BarsantiExplorer.Models.Requests.Comments
{
    public class GetCommentsRequest
    {
        public int? TripId { get; set; }
        public int? Page { get; set; }
        public int? Limit { get; set; }
    }

}
