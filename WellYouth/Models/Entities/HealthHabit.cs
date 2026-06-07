using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WellYouth.Models.Entities
{
    public class HealthHabit
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        [Required]
        [MaxLength(80)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(250)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(50)]
        public string Category { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? TargetFrequency { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ICollection<HealthHabitLog> HealthHabitLogs { get; set; } = new List<HealthHabitLog>();
    }
}
