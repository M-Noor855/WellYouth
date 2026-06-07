using Microsoft.EntityFrameworkCore;
using WellYouth.Data;
using WellYouth.Models.Entities;
using WellYouth.Services.Interfaces;

namespace WellYouth.Services.Implementations
{
    public class HabitService : IHabitService
    {
        private readonly ApplicationDbContext _context;
        private readonly IScoringService _scoringService;

        public HabitService(ApplicationDbContext context, IScoringService scoringService)
        {
            _context = context;
            _scoringService = scoringService;
        }

        public async Task<List<HealthHabit>> GetUserHabitsAsync(string userId)
        {
            return await _context.HealthHabits
                .Where(h => h.UserId == userId && h.IsActive)
                .OrderByDescending(h => h.CreatedAt)
                .ToListAsync();
        }

        public async Task<HealthHabit?> GetHabitByIdAsync(int id)
        {
            return await _context.HealthHabits.FindAsync(id);
        }

        public async Task<bool> AddHabitAsync(HealthHabit habit)
        {
            _context.HealthHabits.Add(habit);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> LogHabitAsync(int habitId, string userId, bool isCompleted, string? notes = null)
        {
            var today = DateTime.UtcNow.Date;
            
            // Check if log already exists for today
            var existingLog = await _context.HealthHabitLogs
                .FirstOrDefaultAsync(l => l.HealthHabitId == habitId && l.LogDate.Date == today);

            bool shouldAwardPoints = false;

            if (existingLog != null)
            {
                if (isCompleted && !existingLog.PointsAwarded) { 
                    shouldAwardPoints = true;
                    existingLog.PointsAwarded = true;
                }

                existingLog.IsCompleted = isCompleted;
                existingLog.Notes = notes;
                _context.HealthHabitLogs.Update(existingLog);
            }
            else
            {
                var newLog = new HealthHabitLog
                {
                    HealthHabitId = habitId,
                    UserId = userId,
                    LogDate = today,
                    IsCompleted = isCompleted,
                    Notes = notes,
                    PointsAwarded = isCompleted,
                    CreatedAt = DateTime.UtcNow
                };
                if (isCompleted) shouldAwardPoints = true;
                _context.HealthHabitLogs.Add(newLog);
            }

            var saved = await _context.SaveChangesAsync() > 0;

            if (saved && shouldAwardPoints)
            {
                // Award points for completing a habit
                await _scoringService.AwardPointsAsync(userId, 5, "Habit completion: " + (await GetHabitByIdAsync(habitId))?.Name);
            }

            return saved;
        }

        public async Task<List<HealthHabitLog>> GetHabitLogsAsync(string userId, DateTime startDate, DateTime endDate)
        {
            return await _context.HealthHabitLogs
                .Include(l => l.HealthHabit)
                .Where(l => l.UserId == userId && l.LogDate >= startDate && l.LogDate <= endDate)
                .OrderByDescending(l => l.LogDate)
                .ToListAsync();
        }
    }
}