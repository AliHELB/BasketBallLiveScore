using System.Collections.Generic;

namespace BasketBall.Server.Models
{
    public class Match
    {
        public int MatchId { get; set; }
        public int NumberOfQuarters { get; set; }
        public int QuarterDuration { get; set; }
        public int TimeoutDuration { get; set; }
        public int CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; } = null!;
        public int HomeTeamId { get; set; }
        public Team HomeTeam { get; set; } = null!;
        public int AwayTeamId { get; set; }
        public Team AwayTeam { get; set; } = null!;
        public List<int> EncoderUserIds { get; set; } = new List<int>();
    }
}
