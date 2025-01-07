namespace BasketBall.Server.Models
{
    public class TimerMatch
    {
        public int TimerMatchId { get; set; }
        public int MatchId { get; set; } 
        public int Quarter { get; set; }
        public int RemainingTime { get; set; } // Temps restant en secondes
    }
}
