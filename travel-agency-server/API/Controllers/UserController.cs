using Domain.Constants;
using Domain.Responses;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using travel_agency_server.Domain.Entities;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _ctx;

        public UserController(UserManager<User> userManager, ApplicationDbContext ctx)
        {
            _userManager = userManager;
            _ctx = ctx;
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers(int limit, int offset)
        {
            var users = _ctx.Users
                .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role)
                .Skip(offset * limit)
                .Take(limit)
                .AsEnumerable()
                .Select(x => new UserResponse
                {
                    Id = x.Id,
                    Name = $"{x.FirstName} {x.LastName}",
                    Email = x.Email,
                    DateJoined = x.DateJoined,
                    ItineraryCreated = x.ItineraryCreated,
                    Roles = x.UserRoles.Select(r => r.Role.NormalizedName).ToList()
                })
                .ToList();
                ;
            return Ok(new { users, total = users.Count });
        }
    }
}
