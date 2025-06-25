using travel_agency_server.Domain.Entities;
using travel_agency_server.Domain.Requests;

namespace Application.Abstracts
{
    public interface IAccountService
    {
        Task RegisterAsync(RegisterRequest registerRequest, string roleName);

        Task<string> LoginAsync(LoginRequest loginRequest);

        Task<string> RefreshTokenAsync(string? refreshToken);

        Task<User?> GetCurrentUser(string? userEmail);

        Task LogoutAsync(string userName);
    }
}
