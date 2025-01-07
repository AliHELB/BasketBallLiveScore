namespace BasketBall.Server.DTOs
{
    public class PlayerDTO
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int PlayerNumber { get; set; }
        public int TeamId { get; set; }
    }
}
