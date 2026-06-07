using WellYouth.Models.Entities;

namespace WellYouth.Services.Interfaces
{
    public interface IHabitService
    {
        Task<List<HealthHabit>> GetUserHabitsAsync(string userId);
        Task<HealthHabit?> GetHabitByIdAsync(int id);
        Task<bool> AddHabitAsync(HealthHabit habit);
        Task<bool> LogHabitAsync(int habitId, string userId, bool isCompleted, string? notes = null);
        Task<List<HealthHabitLog>> GetHabitLogsAsync(string userId, DateTime startDate, DateTime endDate);
    }
}