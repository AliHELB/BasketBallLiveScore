using BasketBall.Server.Data;
using BasketBall.Server.Models;

namespace BasketBall.Server.Services
{
    public class MatchService
    {
        private readonly ApplicationDbContext _context;

        public MatchService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Match AddMatch(
            int quarters,
            int quarterDuration,
            int timeoutDuration,
            int createdByUserId,
            int homeTeamId,
            int awayTeamId,
            List<int> encoderUserIds)
        {
            var match = new Match
            {
                NumberOfQuarters = quarters,
                QuarterDuration = quarterDuration,
                TimeoutDuration = timeoutDuration,
                CreatedByUserId = createdByUserId,
                HomeTeamId = homeTeamId,
                AwayTeamId = awayTeamId,
                EncoderUserIds = encoderUserIds
            };

            _context.Matches.Add(match);
            _context.SaveChanges();

            // Ajouter les lignes TimerMatch pour chaque quart
            CreateTimersForMatch(match.MatchId, quarters, quarterDuration);

            return match;
        }

        private void CreateTimersForMatch(int matchId, int quarters, int quarterDuration)
        {
            var timers = new List<TimerMatch>();

            for (int quarter = 1; quarter <= quarters; quarter++)
            {
                timers.Add(new TimerMatch
                {
                    MatchId = matchId,
                    Quarter = quarter,
                    RemainingTime = quarterDuration * 60 // Convertir les minutes en secondes
                });
            }

            _context.TimerMatches.AddRange(timers);
            _context.SaveChanges();
        }

        public List<Match> GetAllMatches()
        {
            return _context.Matches.ToList();
        }

        public Match? GetMatchById(int matchId)
        {
            return _context.Matches.FirstOrDefault(m => m.MatchId == matchId);
        }

    }
}
