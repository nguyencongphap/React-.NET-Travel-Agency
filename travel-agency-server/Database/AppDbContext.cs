using Microsoft.EntityFrameworkCore;
using travel_agency_server.Database.Models;

namespace travel_agency_server.Database
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
    }

    // TODO: Use Fluent API to make User.username unique in OnModelCreating
}
