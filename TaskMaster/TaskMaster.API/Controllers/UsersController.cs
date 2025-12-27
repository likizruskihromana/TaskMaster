using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskMaster.Application.Services;
using TaskMaster.Domain.Entities;

namespace TaskMaster.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly UserManager<User> userManager;
        public UsersController(UserManager<User> userManager, IUserService userService)
        {
            this.userManager = userManager;
            this.userService = userService;
        }

        [HttpGet("profile")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetProfile()
        {
            // Uzmi email iz JWT tokena
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized(new
                {
                    message = "Email claim not found in token"
                });
            }

            var user = await userService.GetProfileAsync(email);

            if (user == null)
            {
                return NotFound(new
                {
                    message = "User not found"
                });
            }

            return Ok(new
            {
                user,
                message = "Profile data retrieved successfully"
            });
        }
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllUsers()
        {
            
            var users =await userService.GetAllUsersAsync();
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