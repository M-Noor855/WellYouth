using Microsoft.EntityFrameworkCore;
using WellYouth.Data;
using WellYouth.Models.Entities;
using WellYouth.Services.Interfaces;

namespace WellYouth.Services.Implementations
{
    public class ScoringService : IScoringService
    {
        private readonly ApplicationDbContext _context;

        public ScoringService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AwardPointsAsync(string userId, int points, string reason)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            var entry = new HealthScoreEntry
            {
                UserId = userId,
                Score = points,
                Reason = reason,
                ScoreDate = DateTime.UtcNow
            };

            user.HealthScore += points;
            _context.HealthScoreEntries.Add(entry);
            _context.Users.Update(user);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> CalculateDailyScoreAsync(string userId, DateTime date)
        {
            var start = date.Date;
            var end = start.AddDays(1).AddTicks(-1);

            return await _context.HealthScoreEntries
                .Where(e => e.UserId == userId && e.ScoreDate >= start && e.ScoreDate <= end)
                .SumAsync(e => e.Score);
        }

        public async Task<int> GetUserTotalScoreAsync(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            return user?.HealthScore ?? 0;
        }

        public async Task<List<HealthScoreEntry>> GetRecentScoreEntriesAsync(string userId, int count = 10)
        {
            return await _context.HealthScoreEntries
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.ScoreDate)
                .Take(count)
                .ToListAsync();
        }
    }
}