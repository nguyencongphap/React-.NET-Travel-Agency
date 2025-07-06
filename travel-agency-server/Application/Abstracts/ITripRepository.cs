using Domain.Entities;

namespace Application.Abstracts
{
    public interface ITripRepository
    {
        Task<Trip?> GetTripById(int id);
        Task<IEnumerable<Trip>> GetAllTrips(int limit, int offset);
        Task<Trip> AddAsync(Trip trip);
    }
}
