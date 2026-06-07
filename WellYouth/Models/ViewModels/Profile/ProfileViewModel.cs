using WellYouth.Models.Entities;

namespace WellYouth.Models.ViewModels.Profile
{
    public class ProfileViewModel
    {
        public ApplicationUser User { get; set; } = new();
        public List<HealthScoreEntry> RecentScores { get; set; } = new();
        public List<string> Suggestions { get; set; } = new();
        public List<SpecialistContactRequest> ContactRequests { get; set; } = new();
        public int HabitCompletionCount { get; set; }
    }
}