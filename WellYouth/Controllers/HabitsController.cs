using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WellYouth.Models.Entities;
using WellYouth.Models.ViewModels.Habits;
using WellYouth.Services.Interfaces;

namespace WellYouth.Controllers
{
    [Authorize]
    public class HabitsController : Controller
    {
        private readonly IHabitService _habitService;
        private readonly IScoringService _scoringService;
        private readonly UserManager<ApplicationUser> _userManager;

        public HabitsController(
            IHabitService habitService,
            IScoringService scoringService,
            UserManager<ApplicationUser> userManager)
        {
            _habitService = habitService;
            _scoringService = scoringService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return Challenge();

            var habits = await _habitService.GetUserHabitsAsync(userId);
            var today = DateTime.UtcNow.Date;
            var logs = await _habitService.GetHabitLogsAsync(userId, today, today);
            var score = await _scoringService.GetUserTotalScoreAsync(userId);

            var viewModel = new HabitsViewModel
            {
                UserHabits = habits,
                RecentLogs = logs,
                CurrentScore = score,
                SelectedDate = today
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateHabitViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateHabitViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                if (userId == null) return Challenge();

                var habit = new HealthHabit
                {
                    UserId = userId,
                    Name = model.Name,
                    Description = model.Description,
                    Category = model.Category,
                    TargetFrequency = model.TargetFrequency,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _habitService.AddHabitAsync(habit);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error saving habit.");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Log(int habitId, bool isCompleted, string? notes = null)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return Unauthorized();

            var result = await _habitService.LogHabitAsync(habitId, userId, isCompleted, notes);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            
            TempData["Error"] = "Could not log habit status.";
            return RedirectToAction(nameof(Index));
        }
    }
}