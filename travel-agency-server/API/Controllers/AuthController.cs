using Application.Abstracts;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using travel_agency_server.Domain.Requests;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AuthController(
            IAccountService accountService
        )
        {
            _accountService = accountService;
        }

        [HttpPost("/register")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            await _accountService.RegisterAsync(registerRequest);

            return Ok();
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var accessToken = await _accountService.LoginAsync(loginRequest);

            return Ok(new { accessToken });
        }

        [HttpPost("/refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            // get cookie containing the refresh token from client using HttpContext
            var refreshToken = HttpContext.Request.Cookies["REFRESH_TOKEN"];

            var accessToken = await _accountService.RefreshTokenAsync(refreshToken);

            return Ok(new { accessToken });
        }

        [HttpGet("/me")]
        [Authorize] // Ensures only logged-in users can access
        public IActionResult GetCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var userName = User.Identity?.Name;
            var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

            if (userId == null)
            {
                return Unauthorized();
            }

            return Ok(new
            {
                Id = userId,
                Email = userEmail,
                Name = userName,
                Roles = roles
            });
        }

        // TODO: DEL LATER. For testing authorization
        [Authorize]
        [HttpGet("/movies")]
        public async Task<IActionResult> GetMovies()
        {
            return Ok(new List<string> { "Matrix " });
        }

        [Authorize(Roles = IdentityRoleConstants.Admin)]
        [HttpGet("/only-admin")]
        public async Task<IActionResult> OnlyAdmin()
        {
            return Ok(new { Message = "You're an admin" });
        }

        [Authorize(Roles = IdentityRoleConstants.User)]
        [HttpGet("/only-user")]
        public async Task<IActionResult> OnlyUser()
        {
            return Ok(new { Message = "You're only a user" });
        }

    }
}
