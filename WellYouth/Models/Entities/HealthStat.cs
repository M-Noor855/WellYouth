using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WellYouth.Models.Entities
{
    public class HealthStat
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        [Required]
        [MaxLength(50)]
        public string StatType { get; set; } = string.Empty;

        [Required]
        public double Value { get; set; }

        [MaxLength(20)]
        public string? Unit { get; set; }

        public DateTime RecordedAt { get; set; } = DateTime.UtcNow;
    }
}
