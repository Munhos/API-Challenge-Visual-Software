using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services.UsersService;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UsersController : ControllerBase
    {
        private readonly UsersService _usersService;

        public UsersController(UsersService userService)
        {
            _usersService = userService;
        }

        [HttpPost("newUser")]
        public async Task<ActionResult> CreateNewUser([FromBody] UserModel user)
        {
            if (user == null)
            {
                return BadRequest("Invalid user");
            }

            var result = await _usersService.CreateNewUser(user);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginUser([FromBody] UserModel user)
        {
            if (user == null)
            {
                return BadRequest("Invalid user");
            }

            var result = await _usersService.LoginUser(user);

            return Ok(result);
        }
    }
}
