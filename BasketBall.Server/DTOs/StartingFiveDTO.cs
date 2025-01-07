namespace BasketBall.Server.DTOs
{
    public class StartingFiveDTO
    {
        public int MatchId { get; set; }
        public int TeamId { get; set; }
        public List<int> PlayerIds { get; set; } = new List<int>();
    }
}
