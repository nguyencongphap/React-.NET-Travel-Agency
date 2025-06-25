using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace travel_agency_server.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiresAtUtc { get; set; }

        public DateTime DateJoined { get; set; }
        public int ItineraryCreated { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }

        public static User Create(
            string email, 
            string firstName, 
            string lastName,
            DateTime dateJoined,
            int itineraryCreated
        )
        {
            return new User
            {
                Email = email,
                UserName = email, // identity api requires us to specify Username
                FirstName = firstName,
                LastName = lastName,
                DateJoined = dateJoined,
                ItineraryCreated = itineraryCreated
            };
        }

        public override string ToString()
        {
            return FirstName + " " + LastName;
        }

    }
}
