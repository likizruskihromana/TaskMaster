using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TaskMaster.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
 
        [HttpGet("profile")]
        [Authorize] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var email = User.FindFirstValue(ClaimTypes.Email);
            var name = User.FindFirstValue(ClaimTypes.Name);

            return Ok(new
            {
                userId,
                email,
                name,
                message = "This is a protected endpoint! You are authenticated!"
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