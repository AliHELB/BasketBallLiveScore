namespace BasketBall.Server.DTOs
{
    public class BasketEventDto
    {
        public int MatchId { get; set; }
        public int PlayerId { get; set; }
        public int Quarter { get; set; }
        public int Points { get; set; }
        public int Timer { get; set; }
    }

}
