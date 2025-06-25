using Domain.Constants;
using Domain.Entities;
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
        private readonly RoleManager<Role> _roleManager;
        private readonly ApplicationDbContext _ctx;

        public UserController(UserManager<User> userManager, ApplicationDbContext ctx)
        {
            _userManager = userManager;
            _ctx = ctx;
        }

        [Authorize(Roles = IdentityRoleConstants.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var test = await _userManager.GetUsersInRoleAsync(IdentityRoleConstants.Admin);
            var test1 = _ctx.Roles.ToList();
            var test2 = _ctx.Users
                            .Include(x => x.UserRoles)
                            .ThenInclude(x => x.Role)
                            .ToList();

            var users = _userManager.Users
                .AsEnumerable()
                .Select(x => new UserResponse
                {
                    Id = x.Id,
                    Name = $"{x.FirstName} {x.LastName}",
                    Email = x.Email,
                    DateJoined = x.DateJoined,
                    ItineraryCreated = x.ItineraryCreated,
                })
                .ToList();
                ;
            return Ok(users);

            return Ok();
        }
    }
}
