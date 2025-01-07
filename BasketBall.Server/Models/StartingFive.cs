using System.ComponentModel.DataAnnotations.Schema;

namespace BasketBall.Server.Models
{
    [Table("StartingFive")]
    public class StartingFive
    {
        public int StartingFiveId { get; set; }
        public int MatchId { get; set; }
        public Match Match { get; set; } = null!;
        public int TeamId { get; set; }
        public Team Team { get; set; } = null!;
        public int PlayerId { get; set; }
        public Player Player { get; set; } = null!;
    }
}
