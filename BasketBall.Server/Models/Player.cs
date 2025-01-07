namespace BasketBall.Server.Models
{
    public class Player
    {
        public int PlayerId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int PlayerNumber { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; } = null!;
    }
}
