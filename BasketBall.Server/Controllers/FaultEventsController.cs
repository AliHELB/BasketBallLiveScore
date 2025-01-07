using BasketBall.Server.DTOs;
using BasketBall.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BasketBall.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FaultEventsController : ControllerBase
    {
        private readonly FaultEventService _service;

        public FaultEventsController(FaultEventService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFaultEvents()
        {
            return Ok(await _service.GetAllFaultEventsAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFaultEventById(int id)
        {
            var faultEvent = await _service.GetFaultEventByIdAsync(id);
            if (faultEvent == null) return NotFound();
            return Ok(faultEvent);
        }

        [HttpGet("match/{matchId}")]
        public async Task<IActionResult> GetFaultEventsByMatchId(int matchId)
        {
            var events = await _service.GetFaultEventsByMatchIdAsync(matchId);
            return Ok(events);
        }


        [HttpPost]
        public async Task<IActionResult> CreateFaultEvent([FromBody] FaultEventDto eventDto)
        {
            await _service.CreateFaultEventAsync(eventDto);

            var hubContext = (IHubContext<MatchEventsHub>)HttpContext.RequestServices.GetService(typeof(IHubContext<MatchEventsHub>));
            var newEvent = new
            {
                MatchId = eventDto.MatchId,
                PlayerId = eventDto.PlayerId,
                Quarter = eventDto.Quarter,
                FaultType = eventDto.FaultType,
                Timer = eventDto.Timer
            };

            await hubContext.Clients.Group(eventDto.MatchId.ToString()).SendAsync("FaultEventAdded", newEvent);

            return CreatedAtAction(nameof(GetFaultEventById), new { id = eventDto.MatchId }, eventDto);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFaultEvent(int id, [FromBody] FaultEventDto eventDto)
        {
            await _service.UpdateFaultEventAsync(id, eventDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFaultEvent(int id)
        {
            await _service.DeleteFaultEventAsync(id);
            return NoContent();
        }
    }

}
