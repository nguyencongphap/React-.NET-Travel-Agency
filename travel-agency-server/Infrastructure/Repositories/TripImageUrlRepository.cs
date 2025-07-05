using Application.Abstracts;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class TripImageUrlRepository : ITripImageUrlRepository
    {
        private readonly ApplicationDbContext _ctx;

        public TripImageUrlRepository(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public TripImageUrl Add(TripImageUrl tripImageUrl)
        {
            _ctx.TripImageUrls.Add(tripImageUrl);
            _ctx.SaveChanges();
            return tripImageUrl;
        }

        public async Task<TripImageUrl> AddAsync(TripImageUrl tripImageUrl)
        {
            _ctx.TripImageUrls.Add(tripImageUrl);
            await _ctx.SaveChangesAsync();
            return tripImageUrl;
        }
    }
}
