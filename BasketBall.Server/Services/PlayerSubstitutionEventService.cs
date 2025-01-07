using BasketBall.Server.Data;
using BasketBall.Server.DTOs;
using BasketBall.Server.Models;
using Microsoft.EntityFrameworkCore;


public class PlayerSubstitutionEventService
{
    private readonly ApplicationDbContext _context;

    public PlayerSubstitutionEventService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<PlayerSubstitutionEventDto>> GetAllAsync()
    {
        return await _context.PlayerSubstitutionEvents
            .Select(e => new PlayerSubstitutionEventDto
            {
                MatchId = e.MatchId,
                PlayerOutId = e.PlayerOutId,
                PlayerInId = e.PlayerInId,
                Quarter = e.Quarter,
                Timer = e.Timer
            })
            .ToListAsync();
    }

    public async Task<PlayerSubstitutionEventDto> GetByIdAsync(int id)
    {
        var substitution = await _context.PlayerSubstitutionEvents.FindAsync(id);
        if (substitution == null) return null;

        return new PlayerSubstitutionEventDto
        {
            MatchId = substitution.MatchId,
            PlayerOutId = substitution.PlayerOutId,
            PlayerInId = substitution.PlayerInId,
            Quarter = substitution.Quarter,
            Timer = substitution.Timer
        };
    }
    public async Task<List<PlayerSubstitutionEventDto>> GetByMatchIdAsync(int matchId)
    {
        return await _context.PlayerSubstitutionEvents
            .Where(e => e.MatchId == matchId)
            .Select(e => new PlayerSubstitutionEventDto
            {
                MatchId = e.MatchId,
                PlayerOutId = e.PlayerOutId,
                PlayerInId = e.PlayerInId,
                Quarter = e.Quarter,
                Timer = e.Timer
            })
            .ToListAsync();
    }

    public async Task<bool> AddAsync(PlayerSubstitutionEventDto dto)
    {
        var substitution = new PlayerSubstitutionEvent
        {
            MatchId = dto.MatchId,
            PlayerOutId = dto.PlayerOutId,
            PlayerInId = dto.PlayerInId,
            Quarter = dto.Quarter,
            Timer = dto.Timer
        };

        _context.PlayerSubstitutionEvents.Add(substitution);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateAsync(int id, PlayerSubstitutionEventDto dto)
    {
        var substitution = await _context.PlayerSubstitutionEvents.FindAsync(id);
        if (substitution == null) return false;

        substitution.MatchId = dto.MatchId;
        substitution.PlayerOutId = dto.PlayerOutId;
        substitution.PlayerInId = dto.PlayerInId;
        substitution.Quarter = dto.Quarter;
        substitution.Timer = dto.Timer;

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var substitution = await _context.PlayerSubstitutionEvents.FindAsync(id);
        if (substitution == null) return false;

        _context.PlayerSubstitutionEvents.Remove(substitution);
        return await _context.SaveChangesAsync() > 0;
    }
}
