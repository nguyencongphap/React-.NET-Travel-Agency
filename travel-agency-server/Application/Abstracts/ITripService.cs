using Domain.Entities;

namespace Application.Abstracts
{
    public interface ITripService
    {
        Task<Trip> CreateTripAsync(
            string tripDetail,
            Guid userId,
            List<string> imageUrls
        );
    }
}
