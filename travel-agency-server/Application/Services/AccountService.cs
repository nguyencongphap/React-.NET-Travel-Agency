using Application.Abstracts;
using Domain.Constants;
using Domain.Enums;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using travel_agency_server.Domain.Entities;
using travel_agency_server.Domain.Requests;

namespace Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAuthTokenProcessor _authTokenProcessor;
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IApplicationDbContext _ctx;

        public AccountService(IAuthTokenProcessor authTokenProcessor, UserManager<User> userManager, IUserRepository userRepository, IApplicationDbContext ctx)
        {
            _authTokenProcessor = authTokenProcessor;
            _userManager = userManager;
            _userRepository = userRepository;
            _ctx = ctx;
        }

        public async Task RegisterAsync(RegisterRequest registerRequest, string roleName)
        {
            // TODO: refactor this with FluentValidation
            // check if user already exists in db
            var userExists = await _userManager.FindByEmailAsync(registerRequest.Email) != null;

            if (userExists)
            {
                throw new UserAlreadyExistsException(email: registerRequest.Email);
            }

            var user = User.Create(
                registerRequest.Email,
                registerRequest.FirstName,
                registerRequest.LastName,
                DateTime.UtcNow,
                0
            );
            // use the provided hasher to hash the password
            // give it the entity and password
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, registerRequest.Password);

            // insert user into db
            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
            {
                throw new RegistrationFailedException(result.Errors.Select(x => x.Description));
            }

            // Associate role to the user
            // A row will be added to the AspNetUserRoles table to tie the AspNetUsers and the AspNetRoles together
            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<string> LoginAsync(LoginRequest loginRequest)
        {
            var user = await _ctx.Users
                            .Include(x => x.UserRoles)
                            .ThenInclude(x => x.Role)
                            .FirstOrDefaultAsync(x => x.UserName == loginRequest.Username);

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequest.Password))
            {
                throw new LoginFailedException(loginRequest.Username);
            }

            var accessToken = await GenerateNewTokensForUser(user);

            return accessToken;
        }

        // Refresh the access token with a valid refresh token
        public async Task<string> RefreshTokenAsync(string? refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken)) throw new RefreshTokenException("Refresh token is missing.");

            // get user associated with this refresh token
            var user = await _userRepository.GetUserByRefreshTokenAsync(refreshToken);

            if (user == null) throw new RefreshTokenException("Unabe to retrieve user for refresh token");

            // check if refresh token is expired, if yes, they'll have to login again
            if (user.RefreshTokenExpiresAtUtc < DateTime.UtcNow) throw new RefreshTokenException("Refresh token is expired.");

            var accessToken = await GenerateNewTokensForUser(user);

            return accessToken;
        }

        public async Task<User?> GetCurrentUser(string? userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail!);

            return user;
        }

        public async Task LogoutAsync(string? userEmail)
        {
            if (string.IsNullOrEmpty(userEmail))
            {
                throw new Exception("Unabe to sign this user out");
            }

            var user = await _userManager.FindByEmailAsync(userEmail!);

            if (user == null)
            {
                throw new Exception("Unabe to find this user");
            }

            // Expire all tokens of user
            var refreshTokenExpirationDateInUtc = DateTime.UtcNow.AddDays(-1);

            user.RefreshToken = null;
            user.RefreshTokenExpiresAtUtc = refreshTokenExpirationDateInUtc;

            // update user in db to save the expired refresh token
            await _userManager.UpdateAsync(user);

            // return access token and refresh token stored in http-only cookies to client
            _authTokenProcessor.WriteAuthTokenAsHttpOnlyCookie("ACCESS_TOKEN", "", refreshTokenExpirationDateInUtc);
            _authTokenProcessor.WriteAuthTokenAsHttpOnlyCookie("REFRESH_TOKEN", "", refreshTokenExpirationDateInUtc);
        }

        private async Task<string> GenerateNewTokensForUser(User user)
        {
            var roles = user.UserRoles.Select(x => x.Role.NormalizedName).ToList();

            // create access token
            var (jwtToken, expirationDateInUtc) = _authTokenProcessor.GenerateJwtToken(user, roles);

            // create refresh token
            var refreshTokenValue = _authTokenProcessor.GenerateRefreshToken();
            var refreshTokenExpirationDateInUtc = DateTime.UtcNow.AddDays(7); // can move this to appesttings

            user.RefreshToken = refreshTokenValue;
            user.RefreshTokenExpiresAtUtc = refreshTokenExpirationDateInUtc;

            // update user in db to save the new refresh token
            await _userManager.UpdateAsync(user);

            // return access token and refresh token stored in http-only cookies to client
            _authTokenProcessor.WriteAuthTokenAsHttpOnlyCookie("ACCESS_TOKEN", jwtToken, expirationDateInUtc);
            _authTokenProcessor.WriteAuthTokenAsHttpOnlyCookie("REFRESH_TOKEN", user.RefreshToken, refreshTokenExpirationDateInUtc);

            return jwtToken;
        }

        private string GetIdentityRoleName(Role role)
        {
            return role switch
            {
                Role.User => IdentityRoleConstants.User,
                _ => throw new ArgumentOutOfRangeException(nameof(role), role, "Provided role is not supported"),
            };
        }
    }
}
