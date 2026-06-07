using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WellYouth.Models.Entities
{
    public class CommunityGroupMember
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

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}
