using System.ComponentModel.DataAnnotations;

namespace WellYouth.Models.Entities
{
    public class WellnessActivity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(50)]
        public string ActivityType { get; set; } = string.Empty;

        public int PointsValue { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public ICollection<UserActivityLog> UserActivityLogs { get; set; } = new List<UserActivityLog>();
    }
}
