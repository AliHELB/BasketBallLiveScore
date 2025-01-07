using System.ComponentModel.DataAnnotations;

namespace BasketBall.Server.Models
{
    public class BasketEvent
    {
        [Key]
        public int EventId { get; set; }
        public int MatchId { get; set; }
        public int PlayerId { get; set; }
        public int Quarter { get; set; }
        public int Points { get; set; }
        public int Timer { get; set; }
    }
}
