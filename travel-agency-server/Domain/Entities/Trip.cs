using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Trip
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string TripDetail { get; set; }
        public DateTime CreatedAt { get; set; }


        // Foreign key property
        public ICollection<UserTrip> UserTrips { get; set; } = [];

        public virtual ICollection<TripImageUrl> TripImageUrls { get; set; } = [];
    }
}
