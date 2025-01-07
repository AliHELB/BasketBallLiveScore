using BasketBall.Server.Data;
using BasketBall.Server.DTOs;
using BasketBall.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace BasketBall.Server.Services
{
    public class FaultEventService
    {
        private readonly ApplicationDbContext _context;

        public FaultEventService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FaultEvent>> GetAllFaultEventsAsync()
        {
            return await _context.FaultEvents.ToListAsync();
        }

        public async Task<FaultEvent> GetFaultEventByIdAsync(int id)
        {
            return await _context.FaultEvents.FindAsync(id);
        }

        public async Task<IEnumerable<FaultEvent>> GetFaultEventsByMatchIdAsync(int matchId)
        {
            return await _context.FaultEvents
                .Where(e => e.MatchId == matchId)
                .ToListAsync();
        }


        public async Task CreateFaultEventAsync(FaultEventDto eventDto)
        {
            var faultEvent = new FaultEvent
            {
                MatchId = eventDto.MatchId,
                PlayerId = eventDto.PlayerId,
                Quarter = eventDto.Quarter,
                FaultType = eventDto.FaultType,
                Timer = eventDto.Timer
            };

            _context.FaultEvents.Add(faultEvent);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFaultEventAsync(int id, FaultEventDto eventDto)
        {
            var faultEvent = await _context.FaultEvents.FindAsync(id);
            if (faultEvent == null) throw new KeyNotFoundException();

            faultEvent.MatchId = eventDto.MatchId;
            faultEvent.PlayerId = eventDto.PlayerId;
            faultEvent.Quarter = eventDto.Quarter;
            faultEvent.FaultType = eventDto.FaultType;
            faultEvent.Timer = eventDto.Timer;

            _context.FaultEvents.Update(faultEvent);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFaultEventAsync(int id)
        {
            var faultEvent = await _context.FaultEvents.FindAsync(id);
            if (faultEvent == null) throw new KeyNotFoundException();

            _context.FaultEvents.Remove(faultEvent);
            await _context.SaveChangesAsync();
        }
    }

}
