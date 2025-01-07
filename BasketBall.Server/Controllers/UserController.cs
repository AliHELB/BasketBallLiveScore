using Microsoft.AspNetCore.Mvc;
using BasketBall.Server.Services;
using BasketBall.Server.DTOs;

namespace BasketBall.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] UserDTO userDto)
        {
            if (string.IsNullOrWhiteSpace(userDto.Username) || string.IsNullOrWhiteSpace(userDto.Password))
            {
                return BadRequest("Username and password are required.");
            }

            if (_userService.UserExists(userDto.Username))
            {
                return Conflict("Username already exists.");
            }

            var user = _userService.AddUser(userDto.Username, userDto.Password, userDto.Role);
            return CreatedAtAction(nameof(CreateUser), new { id = user.Id }, user);
        }

        [HttpGet("all")]
        public IActionResult GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpPut("{id}/role")]
        public IActionResult UpdateUserRole(int id, [FromBody] RoleUpdateDTO roleUpdateDto)
        {
            if (string.IsNullOrWhiteSpace(roleUpdateDto.NewRole))
            {
                return BadRequest("Role cannot be empty.");
            }

            var user = _userService.UpdateUserRole(id, roleUpdateDto.NewRole);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(user);
        }
    }
}