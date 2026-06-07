using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WellYouth.Models.Entities;
using WellYouth.Services.Interfaces;

namespace WellYouth.Controllers
{
    [Authorize]
    public class AssistantController : Controller
    {
        private readonly IAssistantService _assistantService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AssistantController(IAssistantService assistantService, UserManager<ApplicationUser> userManager)
        {
            _assistantService = assistantService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int? id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return Challenge();

            var conversations = await _assistantService.GetUserConversationsAsync(userId);
            ViewBag.Conversations = conversations;

            if (id.HasValue)
            {
                var messages = await _assistantService.GetMessageHistoryAsync(id.Value);
                ViewBag.Messages = messages;
                ViewBag.CurrentConversationId = id.Value;
            }
            else if (conversations.Any())
            {
                return RedirectToAction(nameof(Index), new { id = conversations.First().Id });
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewChat()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return Challenge();

            var conversation = await _assistantService.CreateConversationAsync(userId, "New Conversation");
            return RedirectToAction(nameof(Index), new { id = conversation.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return Challenge();

            var conversations = await _assistantService.GetUserConversationsAsync(userId);
            
            // Security check: Ensure the conversation belongs to the user
            if (!conversations.Any(c => c.Id == id)) return Forbid();

            await _assistantService.DeleteConversationAsync(id);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Ok();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage(int conversationId, string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return BadRequest("Message cannot be empty.");

            try 
            {
                var aiMessage = await _assistantService.ProcessMessageAsync(conversationId, message);

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, content = aiMessage.Content });
                }

                return RedirectToAction(nameof(Index), new { id = conversationId });
            }
            catch (Exception ex)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, error = ex.Message });
                }
                return BadRequest(ex.Message);
            }
        }
    }
}