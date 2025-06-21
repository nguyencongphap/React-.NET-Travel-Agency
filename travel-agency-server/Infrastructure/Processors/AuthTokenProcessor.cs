using Application.Abstracts;
using Infrastructure.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using travel_agency_server.Domain.Entities;

namespace Infrastructure.Processors
{
    public class AuthTokenProcessor : IAuthTokenProcessor
    {
        private readonly JwtOptions _jwtOptions;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthTokenProcessor(
            IOptions<JwtOptions> jwtOptions,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _jwtOptions = jwtOptions.Value;
            _httpContextAccessor = httpContextAccessor;
        }

        public (string jwtToken, DateTime expiresAtUtc) GenerateJwtToken(User user, IList<string> roles)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));

            var credentials = new SigningCredentials(
                signingKey,
                SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // Subject
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique identifier of each token
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.NameId, user.ToString())
            }.Concat(roles.Select(r => new Claim(ClaimTypes.Role, r))); // Add a claim for each role

            var expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationTimeInMinutes);

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: credentials
                );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return (jwtToken, expires);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];

            // get cryptographically secure random number generator
            using var rng = RandomNumberGenerator.Create();
            // fill number array with random bytes
            rng.GetBytes(randomNumber);
            // return the string representation of these bytes
            return Convert.ToBase64String(randomNumber);
        }

        public void WriteAuthTokenAsHttpOnlyCookie(
            string cookieName,
            string token,
            DateTime expiration
        )
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Append(
                cookieName,
                token,
                new CookieOptions
                {
                    HttpOnly = true,
                    Expires = expiration,
                    IsEssential = true, // mark cookie as essential bc that's needed for GDPR compliance when using cookies
                    Secure = true, // make cookie only get transmitted over HTTPS
                    SameSite = SameSiteMode.None, // REQUIRED for cross-origin cookie. Make sure you're using HTTPS on both frontend and backend when using SameSite=None
                    // SameSiteMode.Strict prevents cookie from being sent in a cross site request to mitigate CSRF attack, but our React app is not same site
                }
            );
        }

    }
}
