using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WellYouth.Models.Entities;
using WellYouth.Services.Interfaces;

namespace WellYouth.Controllers
{
    public class SpecialistsController : Controller
    {
        private readonly ISpecialistService _specialistService;
        private readonly UserManager<ApplicationUser> _userManager;

        public SpecialistsController(ISpecialistService specialistService, UserManager<ApplicationUser> userManager)
        {
            _specialistService = specialistService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string? specialty)
        {
            var specialists = string.IsNullOrEmpty(specialty)
                ? await _specialistService.GetAllSpecialistsAsync()
                : await _specialistService.GetSpecialistsBySpecialtyAsync(specialty);

            ViewBag.Specialties = await _specialistService.GetSpecialtiesAsync();
            ViewBag.CurrentSpecialty = specialty;

            return View(specialists);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(int specialistId, string message)
        {
            if (!User.Identity?.IsAuthenticated ?? false) return Challenge();

            var userId = _userManager.GetUserId(User);
            if (userId == null) return Unauthorized();

            var request = new SpecialistContactRequest
            {
                SpecialistId = specialistId,
                UserId = userId,
                Message = message,
                CreatedAt = DateTime.UtcNow,
                Status = "Pending"
            };

            var result = await _specialistService.SubmitContactRequestAsync(request);
            if (result)
            {
                TempData["Success"] = "Your contact request has been sent successfully.";
            }
            else
            {
                TempData["Error"] = "There was an error sending your request.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}