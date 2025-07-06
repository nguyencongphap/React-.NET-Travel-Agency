using Application.Abstracts;
using Domain.Entities;
using Domain.Requests;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TripController : ControllerBase
    {
        private readonly ITripService _tripService;

        public TripController(
            ITripService tripService
        )
        {
            _tripService = tripService;
        }

        [HttpGet]
        public async Task<ActionResult> GetTripById(int id)
        {
            var trip = await _tripService.GetTripById(id);

            if (trip == null)
            {
                return NotFound();
            }

            var tripDto = new TripDto
            {
                Id = trip.Id,
                TripDetail = trip.TripDetail,
                CreatedAt = trip.CreatedAt,
                TripImageUrls = trip.TripImageUrls.Select(img => img.Value).ToList()
            };

            return Ok(tripDto);
        }

        [HttpGet]
        public async Task<ActionResult> GetAllTrips(int limit, int offset)
        {
            var trips = await _tripService.GetAllTrips(limit, offset);
            var tripDtos = trips.Select(trip => new TripDto
            {
                Id = trip.Id,
                TripDetail = trip.TripDetail,
                CreatedAt = trip.CreatedAt,
                TripImageUrls = trip.TripImageUrls.Select(img => img.Value).ToList()
            });

            return Ok(tripDtos);
        }

        [HttpPost]
        public async Task<ActionResult<CreateTripResponse>> CreateTrip(
            [FromBody] CreateTripRequest request
        )
        {
            try
            {
                var createdTrip = await _tripService.CreateTripAsync(
                    request.TripDetail,
                    request.UserId,
                    request.ImageUrls
                );
                return Ok(new CreateTripResponse
                {
                    Id = createdTrip.Id,
                    TripDetail = createdTrip.TripDetail,
                    CreatedAt = createdTrip.CreatedAt.Date,
                    TripImageUrls = createdTrip.TripImageUrls
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }

    public class TripDto
    {
        public int Id { get; set; }
        public string TripDetail { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string> TripImageUrls { get; set; } = [];
        // Don't include UserTrips in the DTO if not needed for the API
    }

    public record CreateTripResponse
    {
        public int Id { get; set; }
        public string TripDetail { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual IEnumerable<TripImageUrl> TripImageUrls { get; set; } = [];
    }
}
