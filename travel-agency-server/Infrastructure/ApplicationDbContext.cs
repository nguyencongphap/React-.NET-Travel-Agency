using Domain.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using travel_agency_server.Domain.Entities;

namespace Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        // Tables
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            /*
            This scans the assembly that contains the ApplicationDbContext type and automatically applies 
            all configuration classes that implement IEntityTypeConfiguration<TEntity>
            so we don't have to manually call modelBuilder.ApplyConfiguration(new CustomConfiguration());
             */
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            base.OnModelCreating(builder);

            builder.Entity<User>()
                .Property(u => u.FirstName).HasMaxLength(256);

            builder.Entity<User>()
                .Property(u => u.LastName).HasMaxLength(256);

            // Seed role data
            builder.Entity<IdentityRole<Guid>>()
                .HasData(new List<IdentityRole<Guid>>() {
                    new IdentityRole<Guid>()
                    {
                        Id = IdentityRoleConstants.AdminRoleGuid,
                        Name = IdentityRoleConstants.Admin,
                        NormalizedName = IdentityRoleConstants.Admin.ToUpper()
                    },
                    new IdentityRole<Guid>()
                    {
                        Id = IdentityRoleConstants.UserRoleGuid,
                        Name = IdentityRoleConstants.User,
                        NormalizedName = IdentityRoleConstants.User.ToUpper()
                    }
                });
        }

    }
}
