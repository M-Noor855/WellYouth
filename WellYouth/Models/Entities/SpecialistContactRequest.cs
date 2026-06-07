using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WellYouth.Models.Entities
{
    public class SpecialistContactRequest
    {
        public int Id { get; set; }

        [Required]
        public int SpecialistId { get; set; }

        [ForeignKey("SpecialistId")]
        public Specialist? Specialist { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Message { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(20)]
        public string Status { get; set; } = "Pending";
    }
}
