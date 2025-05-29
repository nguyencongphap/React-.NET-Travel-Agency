using travel_agency_server.Database.Models;
using travel_agency_server.Database.Models.Dtos.Request;
using travel_agency_server.Database.Models.Dtos.Response;

namespace travel_agency_server.Services.AuthService
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(UserDto request);
        Task<TokenResponseDto?> LoginAsync(UserDto request);
        Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request);

    }
}
