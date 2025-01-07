using BasketBall.Server.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

[ApiController]
[Route("api/[controller]")]
public class PlayerSubstitutionEventController : ControllerBase
{
    private readonly PlayerSubstitutionEventService _service;

    public PlayerSubstitutionEventController(PlayerSubstitutionEventService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpGet("match/{matchId}")]
    public async Task<IActionResult> GetByMatchId(int matchId)
    {
        var substitutions = await _service.GetByMatchIdAsync(matchId);
        return Ok(substitutions);
    }


    [HttpPost]
    public async Task<IActionResult> Add(PlayerSubstitutionEventDto dto)
    {
        if (await _service.AddAsync(dto))
        {
            var hubContext = (IHubContext<MatchEventsHub>)HttpContext.RequestServices.GetService(typeof(IHubContext<MatchEventsHub>));
            var newEvent = new
            {
                MatchId = dto.MatchId,
                PlayerOutId = dto.PlayerOutId,
                PlayerInId = dto.PlayerInId,
                Quarter = dto.Quarter,
                Timer = dto.Timer
            };

            await hubContext.Clients.Group(dto.MatchId.ToString()).SendAsync("SubstitutionEventAdded", newEvent);

            return CreatedAtAction(nameof(GetById), new { id = dto.MatchId }, dto);
        }

        return BadRequest();
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, PlayerSubstitutionEventDto dto)
    {
        if (await _service.UpdateAsync(id, dto)) return NoContent();

        return NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (await _service.DeleteAsync(id)) return NoContent();

        return NotFound();
    }
}
