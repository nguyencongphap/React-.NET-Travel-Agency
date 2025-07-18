﻿using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace travel_agency_server.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiresAtUtc { get; set; }

        public DateTime DateJoined { get; set; }

        public ICollection<UserRole> UserRoles { get; set; } = [];
        public ICollection<UserTrip> UserTrips { get; set; } = [];

        // calculated fields
        //  The [NotMapped] attribute tells Entity Framework to ignore this property when creating the database schema
        [NotMapped]
        public int ItineraryCreated => UserTrips?.Count ?? 0;


        public static User Create(
            string email, 
            string firstName, 
            string lastName,
            DateTime dateJoined
        )
        {
            return new User
            {
                Email = email,
                UserName = email, // identity api requires us to specify Username
                FirstName = firstName,
                LastName = lastName,
                DateJoined = dateJoined,
            };
        }

        public override string ToString()
        {
            return FirstName + " " + LastName;
        }

    }
}
