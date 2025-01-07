using Microsoft.AspNetCore.Mvc;
using BasketBall.Server.Services;
using BasketBall.Server.DTOs;

namespace BasketBall.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamController : ControllerBase
    {
        private readonly TeamService _service;

        public TeamController(TeamService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult AddTeam([FromBody] TeamDTO dto)
        {
            var team = _service.AddTeam(dto.TeamName, dto.CoachName);
            return CreatedAtAction(nameof(AddTeam), new { id = team.TeamId }, team);
        }

        [HttpGet]
        public IActionResult GetAllTeams()
        {
            var teams = _service.GetAllTeams();
            return Ok(teams);
        }

        [HttpGet("{id}")]
        public IActionResult GetTeamById(int id)
        {
            var team = _service.GetTeamById(id);
            if (team == null) return NotFound();

            var dto = new TeamDTO
            {
                TeamName = team.TeamName,
                CoachName = team.CoachName
            };
            return Ok(dto);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTeam(int id, [FromBody] TeamDTO dto)
        {
            var updatedTeam = _service.UpdateTeam(id, dto.TeamName, dto.CoachName);
            if (updatedTeam == null) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTeam(int id)
        {
            var result = _service.DeleteTeam(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
