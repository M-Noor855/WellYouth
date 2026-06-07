using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WellYouth.Data;
using WellYouth.Models.Entities;
using WellYouth.Models.ViewModels.Profile;
using WellYouth.Services.Interfaces;

namespace WellYouth.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IScoringService _scoringService;
        private readonly IRecommendationService _recommendationService;
        private readonly ApplicationDbContext _context;

        public ProfileController(
            UserManager<ApplicationUser> userManager,
            IScoringService scoringService,
            IRecommendationService recommendationService,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _scoringService = scoringService;
            _recommendationService = recommendationService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var scores = await _scoringService.GetRecentScoreEntriesAsync(user.Id);
            var suggestions = await _recommendationService.GetHealthSuggestionsAsync(user.Id);
            
            var completionCount = await _context.HealthHabitLogs
                .CountAsync(l => l.UserId == user.Id && l.IsCompleted);

            var requests = await _context.SpecialistContactRequests
                .Include(r => r.Specialist)
                .Where(r => r.UserId == user.Id)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            var viewModel = new ProfileViewModel
            {
                User = user,
                RecentScores = scores,
                Suggestions = suggestions,
                HabitCompletionCount = completionCount,
                ContactRequests = requests
            };

            return View(viewModel);
        }
    }
}