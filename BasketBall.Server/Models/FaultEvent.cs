using System.ComponentModel.DataAnnotations;

namespace BasketBall.Server.Models
{
    public class FaultEvent
    {
        [Key]
        public int EventId { get; set; }
        public int MatchId { get; set; }
        public int PlayerId { get; set; }
        public int Quarter { get; set; }
        public string FaultType { get; set; }
        public int Timer { get; set; }
    }
}
