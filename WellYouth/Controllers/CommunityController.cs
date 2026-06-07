using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WellYouth.Models.Entities;
using WellYouth.Services.Interfaces;

namespace WellYouth.Controllers
{
    public class CommunityController : Controller
    {
        private readonly ICommunityService _communityService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommunityController(ICommunityService communityService, UserManager<ApplicationUser> userManager)
        {
            _communityService = communityService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var groups = await _communityService.GetAllGroupsAsync();
            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = _userManager.GetUserId(User);
                if (userId != null)
                {
                    ViewBag.UserGroups = await _communityService.GetUserGroupsAsync(userId);
                }
            }
            return View(groups);
        }

        public async Task<IActionResult> Group(int id)
        {
            var group = await _communityService.GetGroupByIdAsync(id);
            if (group == null) return NotFound();

            var posts = await _communityService.GetGroupPostsAsync(id);
            ViewBag.Posts = posts;

            bool isMember = false;
            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = _userManager.GetUserId(User);
                if (userId != null)
                {
                    isMember = await _communityService.IsMemberAsync(id, userId);
                }
            }
            ViewBag.IsMember = isMember;

            return View(group);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Join(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return Challenge();

            await _communityService.JoinGroupAsync(id, userId);
            return RedirectToAction(nameof(Group), new { id });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Post(int groupId, string content)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return Challenge();

            if (!await _communityService.IsMemberAsync(groupId, userId))
            {
                return Unauthorized();
            }

            var post = new CommunityPost
            {
                GroupId = groupId,
                UserId = userId,
                Content = content,
                PostTime = DateTime.UtcNow,
                Status = "Approved" // In a real app, this would be "Pending" for moderation
            };

            await _communityService.CreatePostAsync(post);
            return RedirectToAction(nameof(Group), new { id = groupId });
        }
    }
}