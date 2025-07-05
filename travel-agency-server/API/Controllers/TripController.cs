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

    public record CreateTripResponse
    {
        public int Id { get; set; }
        public string TripDetail { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual IEnumerable<TripImageUrl> TripImageUrls { get; set; } = [];
    }
}
