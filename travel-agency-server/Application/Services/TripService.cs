using Application.Abstracts;
using Domain.Entities;

namespace Application.Services
{
    public class TripService : ITripService
    {
        private readonly ITripRepository _tripRepository;
        private readonly ITripImageUrlRepository _tripImageUrlRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserTripRepository _userTripRepository;

        public TripService(ITripRepository tripRepository, ITripImageUrlRepository tripImageUrlRepository, IUserRepository userRepository, IUserTripRepository userTripRepository)
        {
            _tripRepository = tripRepository;
            _tripImageUrlRepository = tripImageUrlRepository;
            _userRepository = userRepository;
            _userTripRepository = userTripRepository;
        }

        public async Task<Trip?> GetTripById(int id)
        {
            return await _tripRepository.GetTripById(id);
        }

        public async Task<IEnumerable<Trip>> GetAllTrips(int limit, int offset)
        {
            return await _tripRepository.GetAllTrips(limit, offset);
        }

        // Create a new trip for a user
        public async Task<Trip> CreateTripAsync(
            string tripDetail,
            Guid userId, 
            List<string> imageUrls
        )
        {
            // Validate user exists
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("User not found");

            var newTrip = new Trip
            {
                TripDetail = tripDetail,
                CreatedAt = DateTime.UtcNow,
                TripImageUrls = imageUrls.Select(x => new TripImageUrl
                {
                    Value = x, // EF will handle the TripId automatically
                }).ToList()
            };
            var addedNewTrip = await _tripRepository.AddAsync(newTrip);

            var newUserTrip = new UserTrip
            {
                UserId = userId,
                Trip = newTrip,// EF will handle the TripId automatically
            };
            await _userTripRepository.AddAsync(newUserTrip);

            return addedNewTrip;
        }

    }
}
