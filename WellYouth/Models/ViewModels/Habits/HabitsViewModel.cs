using WellYouth.Models.Entities;

namespace WellYouth.Models.ViewModels.Habits
{
    public class HabitsViewModel
    {
        public List<HealthHabit> UserHabits { get; set; } = new();
        public List<HealthHabitLog> RecentLogs { get; set; } = new();
        public int CurrentScore { get; set; }
        public DateTime SelectedDate { get; set; } = DateTime.UtcNow;
    }

    public class CreateHabitViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Category { get; set; } = "General";
        public string? TargetFrequency { get; set; }
    }
}