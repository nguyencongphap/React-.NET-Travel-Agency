using Microsoft.AspNetCore.Identity;
using travel_agency_server.Domain.Entities;

namespace Domain.Entities
{
    public class UserRole : IdentityUserRole<Guid>
    {
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}
