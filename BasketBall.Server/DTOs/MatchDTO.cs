using System.Text.Json.Serialization;

namespace BasketBall.Server.DTOs
{
    public class MatchDTO
    {
        [JsonPropertyName("quarters")]
        public int NumberOfQuarters { get; set; }
        public int QuarterDuration { get; set; }
        public int TimeoutDuration { get; set; }
        public int CreatedByUserId { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public List<int> EncoderUserIds { get; set; } = new List<int>();
    }
}
