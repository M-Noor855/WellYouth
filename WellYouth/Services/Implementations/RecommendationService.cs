using Microsoft.EntityFrameworkCore;
using WellYouth.Data;
using WellYouth.Services.Interfaces;

namespace WellYouth.Services.Implementations
{
    public class RecommendationService : IRecommendationService
    {
        private readonly ApplicationDbContext _context;

        public RecommendationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<string>> GetHealthSuggestionsAsync(string userId)
        {
            var suggestions = new List<string>();
            var today = DateTime.UtcNow.Date;
            var sevenDaysAgo = today.AddDays(-7);

            // Analysis logic
            var recentLogs = await _context.HealthHabitLogs
                .Where(l => l.UserId == userId && l.LogDate >= sevenDaysAgo)
                .ToListAsync();

            var completionRate = recentLogs.Count > 0 
                ? (double)recentLogs.Count(l => l.IsCompleted) / recentLogs.Count 
                : 0;

            if (completionRate < 0.5)
            {
                suggestions.Add("You've completed fewer than half of your planned habits this week. Try setting smaller, more achievable goals.");
            }
            else if (completionRate > 0.8)
            {
                suggestions.Add("Excellent consistency! Consider adding a new challenge to your routine.");
            }

            // Category based suggestions
            var categories = await _context.HealthHabits
                .Where(h => h.UserId == userId && h.IsActive)
                .Select(h => h.Category)
                .Distinct()
                .ToListAsync();

            if (!categories.Contains("Mental"))
            {
                suggestions.Add("Consider adding a mental health habit, like mindfulness or gratitude journaling, to your routine.");
            }
            if (!categories.Contains("Physical"))
            {
                suggestions.Add("Physical activity is key. Even a 10-minute walk can make a difference.");
            }

            if (suggestions.Count == 0)
            {
                suggestions.Add("Keep tracking your habits to receive personalized health suggestions!");
            }

            return suggestions;
        }
    }
}