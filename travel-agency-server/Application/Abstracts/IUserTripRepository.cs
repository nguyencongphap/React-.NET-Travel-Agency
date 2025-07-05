using Domain.Entities;

namespace Application.Abstracts
{
    public interface IUserTripRepository
    {
        Task<UserTrip> AddAsync(UserTrip userTrip);
    }
}
