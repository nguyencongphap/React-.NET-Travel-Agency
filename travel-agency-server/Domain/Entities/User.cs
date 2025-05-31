using Microsoft.AspNetCore.Identity;

namespace travel_agency_server.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiresAtUtc { get; set; }

        public static User Create(string email, string firstName, string lastName)
        {
            return new User
            {
                Email = email,
                UserName = email, // identity api requires us to specify Username
                FirstName = firstName,
                LastName = lastName,
            };
        }

        public override string ToString()
        {
            return FirstName + " " + LastName;
        }

    }
}
