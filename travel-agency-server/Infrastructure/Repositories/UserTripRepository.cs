using Application.Abstracts;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class UserTripRepository : IUserTripRepository
    {
        private readonly ApplicationDbContext _ctx;

        public UserTripRepository(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<UserTrip> AddAsync(UserTrip userTrip)
        {
            _ctx.UserTrips.Add(userTrip);
            await _ctx.SaveChangesAsync();
            return userTrip;
        }

    }
}
