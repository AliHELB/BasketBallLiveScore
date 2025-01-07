using BasketBall.Server.Data;
using BasketBall.Server.DTOs;
using BasketBall.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace BasketBall.Server.Services
{
    public class TimerMatchService
    {
        private readonly ApplicationDbContext _context;

        public TimerMatchService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TimerMatchDto> GetTimerByMatchAndQuarterAsync(int matchId, int quarter)
        {
            var timer = await _context.TimerMatches
                .FirstOrDefaultAsync(t => t.MatchId == matchId && t.Quarter == quarter);
            if (timer == null) return null;

            return new TimerMatchDto
            {
                MatchId = timer.MatchId,
                Quarter = timer.Quarter,
                RemainingTime = timer.RemainingTime
            };
        }

        public async Task CreateOrUpdateTimerAsync(TimerMatchDto timerDto)
        {
            var timer = await _context.TimerMatches
                .FirstOrDefaultAsync(t => t.MatchId == timerDto.MatchId && t.Quarter == timerDto.Quarter);

            if (timer == null)
            {
                timer = new TimerMatch
                {
                    MatchId = timerDto.MatchId,
                    Quarter = timerDto.Quarter,
                    RemainingTime = timerDto.RemainingTime
                };
                _context.TimerMatches.Add(timer);
            }
            else
            {
                timer.RemainingTime = timerDto.RemainingTime;
                _context.TimerMatches.Update(timer);
            }

            await _context.SaveChangesAsync();
        }
    }
}
