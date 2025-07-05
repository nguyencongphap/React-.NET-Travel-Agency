using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using travel_agency_server.Domain.Entities;

namespace Application.Abstracts
{
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<UserRole> UserRoles { get; set; }
        DbSet<UserTrip> UserTrips { get; set; }
        DbSet<Trip> Trips { get; set; }
        DbSet<TripImageUrl> TripImageUrls { get; set; }
    }
}
