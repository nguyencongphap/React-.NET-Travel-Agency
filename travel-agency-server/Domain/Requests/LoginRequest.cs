namespace travel_agency_server.Domain.Requests
{
    public record LoginRequest
    {
        public required string Username { get; init; }
        public required string Password { get; init; }
    }
}
