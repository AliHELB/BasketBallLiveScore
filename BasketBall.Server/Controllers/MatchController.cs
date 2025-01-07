using Microsoft.AspNetCore.Mvc;
using BasketBall.Server.Services;
using BasketBall.Server.DTOs;

namespace BasketBall.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchController : ControllerBase
    {
        private readonly MatchService _service;

        public MatchController(MatchService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult AddMatch([FromBody] MatchDTO dto)
        {
            Console.WriteLine($"Received NumberOfQuarters: {dto.NumberOfQuarters}");
            var match = _service.AddMatch(
                dto.NumberOfQuarters,
                dto.QuarterDuration,
                dto.TimeoutDuration,
                dto.CreatedByUserId,
                dto.HomeTeamId,
                dto.AwayTeamId,
                dto.EncoderUserIds
            );

            return CreatedAtAction(nameof(AddMatch), new { id = match.MatchId }, match);
        }

        [HttpGet]
        public IActionResult GetAllMatches()
        {
            var matches = _service.GetAllMatches();
            return Ok(matches);
        }

        [HttpGet("{matchId}")]
        public IActionResult GetMatchById(int matchId)
        {
            var match = _service.GetMatchById(matchId);
            if (match == null)
            {
                return NotFound($"Match with ID {matchId} not found.");
            }
            return Ok(match);
        }

    }
}
