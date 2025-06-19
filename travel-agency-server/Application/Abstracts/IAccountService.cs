using travel_agency_server.Domain.Requests;

namespace Application.Abstracts
{
    public interface IAccountService
    {
        Task RegisterAsync(RegisterRequest registerRequest);

        Task<string> LoginAsync(LoginRequest loginRequest);

        Task<string> RefreshTokenAsync(string? refreshToken);
    }
}
