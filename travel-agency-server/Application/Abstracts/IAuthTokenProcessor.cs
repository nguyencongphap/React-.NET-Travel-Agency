﻿using travel_agency_server.Domain.Entities;

namespace Application.Abstracts
{
    public interface IAuthTokenProcessor
    {
        public (string jwtToken, DateTime expiresAtUtc) GenerateJwtToken(User user, IList<string> roles);

        public string GenerateRefreshToken();

        public void WriteAuthTokenAsHttpOnlyCookie(
            string cookieName,
            string token,
            DateTime expiration
        );

    }
}
