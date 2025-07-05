using System.ComponentModel.DataAnnotations.Schema;
using travel_agency_server.Domain.Entities;

namespace Domain.Entities
{
    public class UserTrip
    {
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public int TripId { get; set; }
        [ForeignKey("TripId")]
        public virtual Trip Trip { get; set; }
    }
}
