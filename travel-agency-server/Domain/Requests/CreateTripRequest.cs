namespace Domain.Requests
{
    public class CreateTripRequest
    {
        public string TripDetail { get; set; }
        public List<string> ImageUrls { get; set; } = new List<string>();
        public Guid UserId { get; set; }
    }
}
