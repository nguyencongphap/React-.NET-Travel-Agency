using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using travel_agency_server.Database;
using travel_agency_server.Database.Models;
using travel_agency_server.Database.Models.Dtos.Request;
using travel_agency_server.Database.Models.Dtos.Response;

namespace travel_agency_server.Services.AuthService
{
    public class AuthService(
        AppDbContext dbContext,
        IConfiguration configuration
    ) : IAuthService
    {
        public async Task<TokenResponseDto?> LoginAsync(UserDto request)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user is null) return null;

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return await CreateTokenResponse(user);
        }

        public async Task<User?> RegisterAsync(UserDto request)
        {
            // TODO: refactor this validation logic
            if (await dbContext.Users.AnyAsync(u => u.Username == request.Username))
            {
                return null; // User already exists
            }

            var user = new User();
            var hashedPassword = new PasswordHasher<User>()
                .HashPassword(user, request.Password);

            user.Username = request.Username;
            user.PasswordHash = hashedPassword;

            dbContext.Add(user);

            await dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request)
        {
            var user = await ValidateRefreshTokenAsync(request.Username, request.RefreshToken);

            if (user is null) return null;

            return await CreateTokenResponse(user);
        }

        private async Task<TokenResponseDto?> CreateTokenResponse(User user)
        {
            return new TokenResponseDto
            {
                AccessToken = CreateAccessToken(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
            };
        }

        // TODO: Refactor this using Fluent validation
        private async Task<User?> ValidateRefreshTokenAsync(string username, string refreshToken)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
            if (
                user is null ||
                user.RefreshToken != refreshToken ||
                user.RefreshTokenExpiryTime <= DateTime.UtcNow
            ) return null;

            return user;
        }

        private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            var refreshToken = Convert.ToBase64String(randomNumber);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await dbContext.SaveChangesAsync();
            return refreshToken;
        }

        private string CreateAccessToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512); // when using HmacSha512, key must have length 512 bits (64 bytes)

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("AppSettings:Issuer"), // by using this env var, it can check whethe the token given by the client was indeed issued by the server
                audience: configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
