using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BasketBall.Server.Models
{
    [Table("PlayerSubstitutionEvent")]
    public class PlayerSubstitutionEvent
    {
        [Key]
        public int EventId { get; set; }
        public int MatchId { get; set; }
        public int PlayerOutId { get; set; }
        public int PlayerInId { get; set; }
        public int Quarter { get; set; }
        public int Timer { get; set; }
    }
}