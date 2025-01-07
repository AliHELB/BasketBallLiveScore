using BasketBall.Server.Data;
using BasketBall.Server.DTOs;
using BasketBall.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace BasketBall.Server.Services
{
    public class BasketEventService
    {
        private readonly ApplicationDbContext _context;

        public BasketEventService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BasketEvent>> GetAllBasketEventsAsync()
        {
            return await _context.BasketEvents.ToListAsync();
        }

        public async Task<BasketEvent> GetBasketEventByIdAsync(int id)
        {
            return await _context.BasketEvents.FindAsync(id);
        }

        public async Task<IEnumerable<BasketEvent>> GetBasketEventsByMatchIdAsync(int matchId)
        {
            return await _context.BasketEvents
                .Where(e => e.MatchId == matchId)
                .ToListAsync();
        }

        public async Task CreateBasketEventAsync(BasketEventDto eventDto)
        {
            var basketEvent = new BasketEvent
            {
                MatchId = eventDto.MatchId,
                PlayerId = eventDto.PlayerId,
                Quarter = eventDto.Quarter,
                Points = eventDto.Points,
                Timer = eventDto.Timer
            };

            _context.BasketEvents.Add(basketEvent);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBasketEventAsync(int id, BasketEventDto eventDto)
        {
            var basketEvent = await _context.BasketEvents.FindAsync(id);
            if (basketEvent == null) throw new KeyNotFoundException();

            basketEvent.MatchId = eventDto.MatchId;
            basketEvent.PlayerId = eventDto.PlayerId;
            basketEvent.Quarter = eventDto.Quarter;
            basketEvent.Points = eventDto.Points;
            basketEvent.Timer = eventDto.Timer;

            _context.BasketEvents.Update(basketEvent);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBasketEventAsync(int id)
        {
            var basketEvent = await _context.BasketEvents.FindAsync(id);
            if (basketEvent == null) throw new KeyNotFoundException();

            _context.BasketEvents.Remove(basketEvent);
            await _context.SaveChangesAsync();
        }
    }

}
