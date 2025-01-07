using Microsoft.AspNetCore.Mvc;
using BasketBall.Server.Services;
using BasketBall.Server.DTOs;

namespace BasketBall.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly PlayerService _service;

        public PlayerController(PlayerService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult AddPlayer([FromBody] PlayerDTO dto)
        {
            var player = _service.AddPlayer(dto.FirstName, dto.LastName, dto.PlayerNumber, dto.TeamId);
            return CreatedAtAction(nameof(AddPlayer), new { id = player.PlayerId }, player);
        }

        [HttpGet]
        public IActionResult GetAllPlayers()
        {
            var players = _service.GetAllPlayers();
            return Ok(players);
        }

        [HttpGet("{id}")]
        public IActionResult GetPlayerById(int id)
        {
            var player = _service.GetPlayerById(id);
            if (player == null) return NotFound();
            return Ok(player);
        }

        [HttpGet("team/{teamId}")]
        public IActionResult GetPlayersByTeamId(int teamId)
        {
            var players = _service.GetPlayersByTeamId(teamId);
            return Ok(players);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePlayer(int id, [FromBody] PlayerDTO dto)
        {
            var updatedPlayer = _service.UpdatePlayer(id, dto.FirstName, dto.LastName, dto.PlayerNumber, dto.TeamId);
            if (updatedPlayer == null) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePlayer(int id)
        {
            var result = _service.DeletePlayer(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
