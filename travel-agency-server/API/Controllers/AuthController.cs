﻿using Application.Abstracts;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using travel_agency_server.Domain.Requests;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AuthController(
            IAccountService accountService
        )
        {
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            await _accountService.RegisterAsync(registerRequest, IdentityRoleConstants.User);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var accessToken = await _accountService.LoginAsync(loginRequest);

            return Ok(new { accessToken });
        }

        [HttpPost]
        public async Task<IActionResult> RefreshToken()
        {
            // get cookie containing the refresh token from client using HttpContext
            var refreshToken = HttpContext.Request.Cookies["REFRESH_TOKEN"];

            await _accountService.RefreshTokenAsync(refreshToken);

            return Ok();
        }

        [Authorize] // Ensures only logged-in users can access
        [HttpGet]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

            var user = await _accountService.GetCurrentUser(userEmail);

            if (userId == null || user == null)
            {
                return Unauthorized();
            }

            return Ok(new
            {
                Id = userId,
                Email = userEmail,
                Name = $"{user.FirstName} {user.LastName}",
                Roles = roles
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            await _accountService.LogoutAsync(userEmail);

            return Ok(new { message = "Logged out successfully" });
        }

        // TODO: DEL LATER. For testing authorization
        [Authorize]
        [HttpGet("/movies")]
        public async Task<IActionResult> GetMovies()
        {
            return Ok(new List<string> { "Matrix " });
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("/only-admin")]
        public async Task<IActionResult> OnlyAdmin()
        {
            return Ok(new { Message = "You're an admin" });
        }

        [Authorize(Roles = "USER")]
        [HttpGet("/only-user")]
        public async Task<IActionResult> OnlyUser()
        {
            return Ok(new { Message = "You're only a user" });
        }

    }
}
