namespace BasketBall.Server.DTOs
{
    public class FaultEventDto
    {
        public int MatchId { get; set; }
        public int PlayerId { get; set; }
        public int Quarter { get; set; }
        public string FaultType { get; set; }
        public int Timer { get; set; }
    }
}
