using Application.Abstracts;
using Microsoft.EntityFrameworkCore;
using travel_agency_server.Domain.Entities;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _ctx;

        public UserRepository(
            ApplicationDbContext applicationDbContext    
        )
        {
            _ctx = applicationDbContext;
        }

        public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
        {
            var user = await _ctx.Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);

            return user;
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            return await _ctx.Users.FindAsync(id);
        }

    }
}
