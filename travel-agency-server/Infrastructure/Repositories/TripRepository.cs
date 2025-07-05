using Application.Abstracts;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class TripRepository : ITripRepository
    {
        private readonly ApplicationDbContext _ctx;

        public TripRepository(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Trip> GetByIdAsync(int id)
        {
            return await _ctx.Trips.FindAsync(id);
        }

        public async Task<Trip> AddAsync(Trip trip)
        {
            _ctx.Trips.Add(trip);
            await _ctx.SaveChangesAsync();
            return trip;
        }

    }
}
