using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskMaster.Domain.Entities;

namespace TaskMaster.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        public UsersController(UserManager<User> userManager) => this.userManager = userManager;

        [HttpGet("profile")]
        [Authorize] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetProfile()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var name = User.FindFirstValue(ClaimTypes.Name);

            return Ok(new
            {
                email,
                name,
                message = "This is a protected endpoint! You are authenticated!"
            });
        }
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetAllUsers()
        {
            var users = userManager.Users;
                return Ok(new
                {   
                    users,
                    message = "These are all users."
                });
        }
        [HttpGet("public")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetPublicData()
        {
            return Ok(new
            {
                message = "This is a public endpoint. Anyone can access this!",
                timestamp = DateTime.UtcNow
            });
        }
    }
}