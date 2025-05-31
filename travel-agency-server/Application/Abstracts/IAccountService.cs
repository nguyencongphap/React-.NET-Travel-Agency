using travel_agency_server.Domain.Requests;

namespace Application.Abstracts
{
    public interface IAccountService
    {
        Task RegisterAsync(RegisterRequest registerRequest);

        Task LoginAsync(LoginRequest loginRequest);

        Task RefreshTokenAsync(string? refreshToken);
    }
}
