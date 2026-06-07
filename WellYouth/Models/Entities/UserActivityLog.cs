using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WellYouth.Models.Entities
{
    public class UserActivityLog
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        [Required]
        public int ActivityId { get; set; }

        [ForeignKey("ActivityId")]
        public WellnessActivity? Activity { get; set; }

        public DateTime CompletedAt { get; set; } = DateTime.UtcNow;

        public int PointsEarned { get; set; } = 0;
    }
}
