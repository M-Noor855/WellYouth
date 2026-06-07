using System.ComponentModel.DataAnnotations;

namespace WellYouth.Models.Entities
{
    public class CommunityGroup
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [MaxLength(1000)]
        public string? Guidelines { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ICollection<CommunityGroupMember> Members { get; set; } = new List<CommunityGroupMember>();
        public ICollection<CommunityPost> Posts { get; set; } = new List<CommunityPost>();
    }
}
