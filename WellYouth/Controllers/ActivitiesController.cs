using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WellYouth.Data;
using WellYouth.Models.Entities;
using WellYouth.Services.Interfaces;

namespace WellYouth.Controllers
{
    [Authorize]
    public class ActivitiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IScoringService _scoringService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ActivitiesController(
            ApplicationDbContext context, 
            IScoringService scoringService, 
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _scoringService = scoringService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var activities = await _context.WellnessActivities.Where(a => a.IsActive).ToListAsync();
            return View(activities);
        }

        public async Task<IActionResult> Play(int id)
        {
            var activity = await _context.WellnessActivities.FindAsync(id);
            if (activity == null) return NotFound();

            return activity.ActivityType switch
            {
                "Breathing" => View("BreathingTimer", activity),
                "MoodCheckIn" => View("MoodCheckIn", activity),
                "Hydration" => View("Hydration", activity),
                "Mindfulness" => View("Mindfulness", activity),
                "Stretch" => View("Stretch", activity),
                "Gratitude" => View("Gratitude", activity),
                "SleepWindDown" => View("SleepWindDown", activity),
                "PositiveFocus" => View("PositiveFocus", activity),
                _ => View(activity)
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return Challenge();

            var activity = await _context.WellnessActivities.FindAsync(id);
            if (activity == null) return NotFound();

            var log = new UserActivityLog
            {
                UserId = userId,
                ActivityId = id,
                CompletedAt = DateTime.UtcNow,
                PointsEarned = activity.PointsValue
            };

            _context.UserActivityLogs.Add(log);
            await _context.SaveChangesAsync();

            await _scoringService.AwardPointsAsync(userId, activity.PointsValue, $"Activity completed: {activity.Title}");

            TempData["Success"] = $"Awesome! You earned {activity.PointsValue} health points!";
            return RedirectToAction(nameof(Index));
        }
    }
}