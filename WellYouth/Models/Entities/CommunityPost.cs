using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WellYouth.Models.Entities
{
    public class CommunityPost
    {
        public int Id { get; set; }

        [Required]
        public int GroupId { get; set; }

        [ForeignKey("GroupId")]
        public CommunityGroup? Group { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Content { get; set; } = string.Empty;

        public DateTime PostTime { get; set; } = DateTime.UtcNow;

        [MaxLength(20)]
        public string Status { get; set; } = "Pending";

        [MaxLength(500)]
        public string? ModerationReason { get; set; }
    }
}
