using WellYouth.Models.Entities;

namespace WellYouth.Services.Interfaces
{
    public interface IScoringService
    {
        Task<int> CalculateDailyScoreAsync(string userId, DateTime date);
        Task<bool> AwardPointsAsync(string userId, int points, string reason);
        Task<int> GetUserTotalScoreAsync(string userId);
        Task<List<HealthScoreEntry>> GetRecentScoreEntriesAsync(string userId, int count = 10);
    }
}