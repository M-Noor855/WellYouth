using WellYouth.Models.Entities;

namespace WellYouth.Services.Interfaces
{
    public interface IAssistantService
    {
        Task<AssistantConversation> CreateConversationAsync(string userId, string? title = null);
        Task<List<AssistantConversation>> GetUserConversationsAsync(string userId);
        Task<AssistantMessage> ProcessMessageAsync(int conversationId, string userMessage);
        Task<List<AssistantMessage>> GetMessageHistoryAsync(int conversationId);
        Task DeleteConversationAsync(int conversationId);
        Task<string> GenerateGeminiResponseAsync(string userId, string userMessage, List<AssistantMessage> history);
    }
}