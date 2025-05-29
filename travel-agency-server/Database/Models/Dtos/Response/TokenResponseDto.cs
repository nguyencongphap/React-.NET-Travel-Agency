namespace travel_agency_server.Database.Models.Dtos.Response
{
    public class TokenResponseDto
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
