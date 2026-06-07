using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WellYouth.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public DateTime BirthDate { get; set; }

        public int Age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - BirthDate.Year;
                if (BirthDate.Date > today.AddYears(-age)) age--;
                return age;
            }
        }

        public int HealthScore { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ICollection<HealthHabit> HealthHabits { get; set; } = new List<HealthHabit>();
        public ICollection<HealthStat> HealthStats { get; set; } = new List<HealthStat>();
        public ICollection<HealthScoreEntry> HealthScoreEntries { get; set; } = new List<HealthScoreEntry>();
        public ICollection<CommunityGroupMember> CommunityGroupMembers { get; set; } = new List<CommunityGroupMember>();
        public ICollection<CommunityPost> CommunityPosts { get; set; } = new List<CommunityPost>();
        public ICollection<SpecialistContactRequest> SpecialistContactRequests { get; set; } = new List<SpecialistContactRequest>();
        public ICollection<AssistantConversation> AssistantConversations { get; set; } = new List<AssistantConversation>();
        public ICollection<UserActivityLog> UserActivityLogs { get; set; } = new List<UserActivityLog>();
    }
}
