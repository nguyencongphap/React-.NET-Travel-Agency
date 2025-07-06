using Application.Abstracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TripRepository : ITripRepository
    {
        private readonly ApplicationDbContext _ctx;

        public TripRepository(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Trip?> GetTripById(int id)
        {
            return await _ctx.Trips
                .Include(x => x.TripImageUrls)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Trip>> GetAllTrips(int limit, int offset)
        {
            return await _ctx.Trips
                .Include(x => x.TripImageUrls)
                .Skip(offset * limit)
                .Take(limit)
                .OrderBy(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<Trip> AddAsync(Trip trip)
        {
            _ctx.Trips.Add(trip);
            await _ctx.SaveChangesAsync();
            return trip;
        }

    }
}
