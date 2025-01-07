namespace BasketBall.Server.DTOs
{
    public class TimerMatchDto
    {
        public int MatchId { get; set; }
        public int Quarter { get; set; } 
        public int RemainingTime { get; set; }
    }
}
