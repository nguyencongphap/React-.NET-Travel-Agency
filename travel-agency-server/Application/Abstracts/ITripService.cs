using Domain.Entities;

namespace Application.Abstracts
{
    public interface ITripService
    {
        Task<Trip?> GetTripById(int id);
        Task<IEnumerable<Trip>> GetAllTrips(int limit, int offset);
        Task<Trip> CreateTripAsync(
            string tripDetail,
            Guid userId,
            List<string> imageUrls
        );
    }
}
