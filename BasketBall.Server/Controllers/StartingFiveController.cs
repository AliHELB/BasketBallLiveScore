using Microsoft.AspNetCore.Mvc;
using BasketBall.Server.Services;
using BasketBall.Server.DTOs;

namespace BasketBall.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StartingFiveController : ControllerBase
    {
        private readonly StartingFiveService _service;

        public StartingFiveController(StartingFiveService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult AddStartingFive([FromBody] StartingFiveDTO dto)
        {
            _service.AddStartingFive(dto.MatchId, dto.TeamId, dto.PlayerIds);
            return Ok(new
            {
                message = "Starting five added successfully.",
                matchId = dto.MatchId,
                teamId = dto.TeamId,
                playerIds = dto.PlayerIds
            });
        }


        [HttpGet]
        public IActionResult GetAllStartingFives()
        {
            var startingFives = _service.GetAllStartingFives();
            return Ok(startingFives);
        }

        [HttpGet("{id}")]
        public IActionResult GetStartingFiveById(int id)
        {
            var startingFive = _service.GetStartingFiveById(id);
            if (startingFive == null)
                return NotFound("Starting five not found.");
            return Ok(startingFive);
        }

        [HttpGet("byMatch/{matchId}")]
        public IActionResult GetStartingFivesByMatchId(int matchId)
        {
            var startingFives = _service.GetStartingFivesByMatchId(matchId);
            if (startingFives == null || !startingFives.Any())
                return NotFound("No starting fives found for the given match.");
            return Ok(startingFives);
        }

        [HttpGet("byMatchAndTeam/{matchId}/{teamId}")]
        public IActionResult GetPlayersByTeam(int matchId, int teamId)
        {
            var players = _service.GetPlayersByTeam(matchId, teamId);
            if (players == null || !players.Any())
                return NotFound("No players found for the given match and team.");
            return Ok(players);
        }

        [HttpPut("byMatchTeamAndPlayer/{matchId}/{teamId}/{playerId}")]
        public IActionResult UpdatePlayerInStartingFive(int matchId, int teamId, int playerId, [FromBody] StartingFiveDTO dto)
        {
            try
            {
                _service.UpdatePlayerInStartingFive(matchId, teamId, playerId, dto);
                return Ok(new { message = "Player in starting five updated successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }





        [HttpPut("{id}")]
        public IActionResult UpdateStartingFive(int id, [FromBody] StartingFiveDTO dto)
        {
            _service.UpdateStartingFive(id, dto);
            return Ok("Starting five updated successfully.");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStartingFive(int id)
        {
            _service.DeleteStartingFive(id);
            return Ok("Starting five deleted successfully.");
        }
    }
}
