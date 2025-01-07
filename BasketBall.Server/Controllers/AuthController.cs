using Microsoft.AspNetCore.Mvc;
using BasketBall.Server.Services;
using BasketBall.Server.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace BasketBall.Server.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO loginDto)
        {
            if (string.IsNullOrWhiteSpace(loginDto.Username) || string.IsNullOrWhiteSpace(loginDto.Password))
            {
                return BadRequest("Username and password are required.");
            }

            var token = _authService.Authenticate(loginDto.Username, loginDto.Password);

            if (token == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            return Ok(new { Token = token });
        }

        [Authorize(Roles = "admin")]
        [HttpGet("admin-only")]
        public IActionResult AdminOnly()
        {
            return Ok("This is an admin-only endpoint.");
        }
    }
}
