using travel_agency_server.Domain.Entities;

namespace Application.Abstracts
{
    public interface IUserRepository
    {
        Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
        Task<User> GetByIdAsync(Guid id);
    }
}
