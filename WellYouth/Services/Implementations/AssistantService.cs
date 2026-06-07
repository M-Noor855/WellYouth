using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mscc.GenerativeAI;
using Mscc.GenerativeAI.Types;
using WellYouth.Data;
using WellYouth.Models.Entities;
using WellYouth.Services.Interfaces;

namespace WellYouth.Services.Implementations
{
    public class AssistantService : IAssistantService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHabitService _habitService;
        private readonly IScoringService _scoringService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AssistantService(
            ApplicationDbContext context,
            IConfiguration configuration,
            IHabitService habitService,
            IScoringService scoringService,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _configuration = configuration;
            _habitService = habitService;
            _scoringService = scoringService;
            _userManager = userManager;
        }

        public async Task<AssistantConversation> CreateConversationAsync(string userId, string? title = null)
        {
            var conversation = new AssistantConversation
            {
                UserId = userId,
                Title = title ?? "New Chat",
                CreatedAt = DateTime.UtcNow
            };
            _context.AssistantConversations.Add(conversation);
            await _context.SaveChangesAsync();
            return conversation;
        }

        public async Task<List<AssistantConversation>> GetUserConversationsAsync(string userId)
        {
            return await _context.AssistantConversations
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<AssistantMessage> ProcessMessageAsync(int conversationId, string userMessage)
        {
            var conversation = await _context.AssistantConversations.FindAsync(conversationId);
            if (conversation == null) throw new Exception("Conversation not found");

            // 1. Save User Message
            var userMsg = new AssistantMessage
            {
                ConversationId = conversationId,
                Sender = "User",
                Content = userMessage,
                SentAt = DateTime.UtcNow
            };
            _context.AssistantMessages.Add(userMsg);
            await _context.SaveChangesAsync();

            // 2. Get history for context (limit to last 10 for performance/tokens)
            var history = await _context.AssistantMessages
                .Where(m => m.ConversationId == conversationId)
                .OrderByDescending(m => m.SentAt)
                .Take(11) // Including the current message
                .OrderBy(m => m.SentAt)
                .ToListAsync();

            // 3. Generate Gemini Response
            string responseContent = await GenerateGeminiResponseAsync(conversation.UserId, userMessage, history);

            var aiMsg = new AssistantMessage
            {
                ConversationId = conversationId,
                Sender = "Assistant",
                Content = responseContent,
                SentAt = DateTime.UtcNow,
                SafetyCategory = "General"
            };
            _context.AssistantMessages.Add(aiMsg);

            await _context.SaveChangesAsync();
            return aiMsg;
        }

        public async Task<string> GenerateGeminiResponseAsync(string userId, string userMessage, List<AssistantMessage> history)
        {
            var apiKey = _configuration["GEMINI_API_KEY"];
            if (string.IsNullOrEmpty(apiKey) || apiKey == "YOUR_API_KEY_HERE")
            {
                return "I'm currently disconnected from my AI brain. Please make sure my API key is configured correctly.";
            }

            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                var totalScore = await _scoringService.GetUserTotalScoreAsync(userId);
                var recentLogs = await _habitService.GetHabitLogsAsync(userId, DateTime.Today.AddDays(-3), DateTime.Today);

                var habitsSummary = string.Join("\n", recentLogs.Select(l => $"- {l.HealthHabit?.Name}: {(l.IsCompleted ? "Completed" : "Not Completed")} ({l.LogDate.ToShortDateString()})"));

                var systemPrompt = $@"You are the WellYouth AI Assistant, a friendly and supportive wellness coach for young adults.

User context:
- User name: {user?.FullName ?? "User"}
- Current health points: {totalScore}
- Recent habits from the last 3 days:
{habitsSummary}

Your role:
- Give practical wellness guidance.
- Encourage the user without judging them.
- Highlight recent successes when relevant.
- Support the user gently when they missed habits.
- You are not a doctor. For medical symptoms, pain, illness, medication, or urgent health concerns, recommend speaking with a qualified specialist.

Response style:
- Answer the user's question directly first.
- Keep the tone friendly, warm, and simple.
- Keep paragraphs short, maximum 2-3 sentences.
- Avoid long blocks of text.

Formatting rules:
- Format the response in Markdown.
- Separate paragraphs and bullet points with a blank line.
- Use bullet points for advice or tips.
- Each bullet point must start with a bold title.
- Use this bullet format:
* **Title:** Explanation.
";

                var googleAI = new GoogleAI(apiKey);
                // Using the specific model name recommended to fix 404s
                var model = googleAI.GenerativeModel(model: "models/gemini-3.5-flash", systemInstruction: new Content(systemPrompt));

                // Prepare request with history
                var request = new GenerateContentRequest();
                request.Contents = history
                    .Select(m => new Content
                    {
                        Role = m.Sender == "User" ? Role.User : Role.Model,
                        Parts = new List<IPart> { new Part { Text = m.Content } }
                    }).ToList();

                var response = await model.GenerateContent(request);
                return response.Text ?? "I'm sorry, I couldn't generate a response.";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        public async Task<List<AssistantMessage>> GetMessageHistoryAsync(int conversationId)
        {
            return await _context.AssistantMessages
                .Where(m => m.ConversationId == conversationId)
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }

        public async Task DeleteConversationAsync(int conversationId)
        {
            var conversation = await _context.AssistantConversations
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.Id == conversationId);

            if (conversation != null)
            {
                _context.AssistantMessages.RemoveRange(conversation.Messages);
                _context.AssistantConversations.Remove(conversation);
                await _context.SaveChangesAsync();
            }
        }
    }
}