using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WellYouth.Models.Entities
{
    public class AssistantMessage
    {
        public int Id { get; set; }

        [Required]
        public int ConversationId { get; set; }

        [ForeignKey("ConversationId")]
        public AssistantConversation? Conversation { get; set; }

        [Required]
        [MaxLength(20)]
        public string Sender { get; set; } = string.Empty; // "User" or "Assistant"

        [Required]
        public string Content { get; set; } = string.Empty;

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        [MaxLength(50)]
        public string? SafetyCategory { get; set; }
    }
}
