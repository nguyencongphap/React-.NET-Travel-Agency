using Domain.Entities;

namespace Application.Abstracts
{
    public interface ITripRepository
    {
        Task<Trip> GetByIdAsync(int id);

        Task<Trip> AddAsync(Trip trip);
    }
}
