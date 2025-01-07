namespace BasketBall.Server.DTOs
{
    public class PlayerSubstitutionEventDto
    {
        public int MatchId { get; set; }
        public int PlayerOutId { get; set; }
        public int PlayerInId { get; set; }
        public int Quarter { get; set; }
        public int Timer { get; set; }
    }
}
