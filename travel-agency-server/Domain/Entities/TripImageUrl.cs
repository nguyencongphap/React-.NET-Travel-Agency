using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class TripImageUrl
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Value { get; set; }

        // Foreign key
        public int TripId { get; set; }
        [ForeignKey("TripId")]
        // Navigation property - ignore in JSON serialization to avoid circular reference
        [JsonIgnore]
        public virtual Trip Trip { get; set; }
    }
}
