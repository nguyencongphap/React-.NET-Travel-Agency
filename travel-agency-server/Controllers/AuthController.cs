using Microsoft.AspNetCore.Mvc;
using travel_agency_server.Database.Models;
using travel_agency_server.Database.Models.Dtos.Request;
using travel_agency_server.Database.Models.Dtos.Response;
using travel_agency_server.Services.AuthService;

namespace travel_agency_server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController(
        IAuthService _authService
    ) : ControllerBase
    {
        // for testing purposes
        public static User user = new();

        [HttpPost("/register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            var user = await _authService.RegisterAsync(request);
            if (user is null)
            {
                return BadRequest("User already exists or invalid data.");
            }

            return Ok(user);
        }

        [HttpPost("/login")]
        public async Task<ActionResult<TokenResponseDto>> Login(UserDto request)
        {
            var result = await _authService.LoginAsync(request);
            if (result is null)
            {
                return BadRequest("Invalid username or password.");
            }

            return Ok(result);
        }

        [HttpPost("/refreshToken")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
        {
            var result = await _authService.RefreshTokensAsync(request);

            if (
                result is null ||
                result.AccessToken is null ||
                result.RefreshToken is null
            )
            {
                return Unauthorized("Invalid refresh token.");
            }

            return Ok(result);
        }
        
    }
}
