using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WellYouth.Models.Entities
{
    public class HealthHabitLog
    {
        public int Id { get; set; }

        [Required]
        public int HealthHabitId { get; set; }

        [ForeignKey("HealthHabitId")]
        public HealthHabit? HealthHabit { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        [Required]
        public DateTime LogDate { get; set; } = DateTime.Today;

        public bool IsCompleted { get; set; } = false;

        public bool PointsAwarded { get; set; } = false;

        [MaxLength(250)]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
