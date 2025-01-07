using BasketBall.Server.DTOs;
using BasketBall.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BasketBall.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketEventsController : ControllerBase
    {
        private readonly BasketEventService _service;

        public BasketEventsController(BasketEventService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBasketEvents()
        {
            return Ok(await _service.GetAllBasketEventsAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBasketEventById(int id)
        {
            var basketEvent = await _service.GetBasketEventByIdAsync(id);
            if (basketEvent == null) return NotFound();
            return Ok(basketEvent);
        }

        [HttpGet("match/{matchId}")]
        public async Task<IActionResult> GetBasketEventsByMatchId(int matchId)
        {
            var events = await _service.GetBasketEventsByMatchIdAsync(matchId);
            return Ok(events);
        }


        [HttpPost]
        public async Task<IActionResult> CreateBasketEvent([FromBody] BasketEventDto eventDto)
        {
            await _service.CreateBasketEventAsync(eventDto);

            var hubContext = (IHubContext<MatchEventsHub>)HttpContext.RequestServices.GetService(typeof(IHubContext<MatchEventsHub>));
            var newEvent = new
            {
                MatchId = eventDto.MatchId,
                PlayerId = eventDto.PlayerId,
                Quarter = eventDto.Quarter,
                Points = eventDto.Points,
                Timer = eventDto.Timer
            };

            await hubContext.Clients.Group(eventDto.MatchId.ToString()).SendAsync("BasketEventAdded", newEvent);

            return CreatedAtAction(nameof(GetBasketEventById), new { id = eventDto.MatchId }, eventDto);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBasketEvent(int id, [FromBody] BasketEventDto eventDto)
        {
            await _service.UpdateBasketEventAsync(id, eventDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBasketEvent(int id)
        {
            await _service.DeleteBasketEventAsync(id);
            return NoContent();
        }
    }

}
