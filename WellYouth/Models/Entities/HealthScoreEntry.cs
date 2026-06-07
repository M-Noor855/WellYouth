using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WellYouth.Models.Entities
{
    public class HealthScoreEntry
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        [Required]
        public int Score { get; set; }

        [Required]
        [MaxLength(200)]
        public string Reason { get; set; } = string.Empty;

        public DateTime ScoreDate { get; set; } = DateTime.UtcNow;
    }
}
