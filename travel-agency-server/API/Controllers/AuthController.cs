using Application.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            await _accountService.LoginAsync(loginRequest);

            return Ok();
        }

        [HttpPost("/refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            // get cookie containing the refresh token from client using HttpContext
            var refreshToken = HttpContext.Request.Cookies["REFRESH_TOKEN"];

            await _accountService.RefreshTokenAsync(refreshToken);

            return Ok();
        }

        [Authorize]
        [HttpGet("/movies")]
        public async Task<IActionResult> GetMovies()
        {
            return Ok(new List<string> { "Matrix " });
        }

    }
}
