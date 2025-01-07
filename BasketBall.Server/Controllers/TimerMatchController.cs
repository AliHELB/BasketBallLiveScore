using BasketBall.Server.DTOs;
using BasketBall.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace BasketBall.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimerMatchController : ControllerBase
    {
        private readonly TimerMatchService _service;

        public TimerMatchController(TimerMatchService service)
        {
            _service = service;
        }

        [HttpGet("{matchId}/{quarter}")]
        public async Task<IActionResult> GetTimer(int matchId, int quarter)
        {
            var timer = await _service.GetTimerByMatchAndQuarterAsync(matchId, quarter);
            if (timer == null) return NotFound($"No timer found for match ID {matchId} and quarter {quarter}");
            return Ok(timer);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTimer([FromBody] TimerMatchDto timerDto)
        {
            if (timerDto == null) return BadRequest("Invalid timer data");

            await _service.CreateOrUpdateTimerAsync(timerDto);
            return NoContent();
        }
    }
}
