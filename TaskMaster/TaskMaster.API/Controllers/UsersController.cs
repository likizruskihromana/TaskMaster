using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskMaster.Application.DTOs.Profile;
using TaskMaster.Application.Services;
using TaskMaster.Domain.Entities;

namespace TaskMaster.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        public UsersController(IUserService userService)
        {
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
        [HttpGet("user")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserByEmail(
            [FromBody]
            [Required(ErrorMessage ="Email is required")]
            [DataType(DataType.EmailAddress)]   
            string email)
        {

            var user = await userService.GetUserByEmailAsync(email);
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
        [HttpGet("this/user")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserById()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userService.GetUserByEmailAsync(id);
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
        [HttpPut("profile")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var updatedUser = await userService.UpdateProfileAsync(int.Parse(id), dto);
            if(updatedUser == null)
            {
                return NotFound(new
                {
                    message="User not found"
                });
            }
            return Ok(new
            {
                updatedUser,
                message="Profile data changed successfully"
            });
        }
        [HttpPut("password")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto dto)
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                var updatedUser = await userService.UpdatePasswordAsync(int.Parse(id), dto);
                if (!updatedUser)
                {
                    return NotFound(new
                    {
                        message = "Password is not changed 1."
                    });
                }
                return Ok(new
                {
                    updatedUser,
                    message = "Profile password changed successfully"
                });
            }
            catch(Exception err)
            {
                Console.WriteLine(err);
            }
            return NotFound(new
            {
                message = "Password is not changed."
            });
        }
    }
}