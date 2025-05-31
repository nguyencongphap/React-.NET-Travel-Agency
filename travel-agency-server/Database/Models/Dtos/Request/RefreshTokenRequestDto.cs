namespace travel_agency_server.Database.Models.Dtos.Request
{
    public class RefreshTokenRequestDto
    {
        public string Username { get; set; }
        public required string RefreshToken { get; set; }
    }
}
