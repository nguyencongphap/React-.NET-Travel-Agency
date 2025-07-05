using Domain.Entities;

namespace Application.Abstracts
{
    public interface ITripImageUrlRepository
    {
        TripImageUrl Add(TripImageUrl tripImageUrl);
        Task<TripImageUrl> AddAsync(TripImageUrl tripImageUrl);
    }
}
