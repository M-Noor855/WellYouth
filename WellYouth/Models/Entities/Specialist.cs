using System.ComponentModel.DataAnnotations;

namespace WellYouth.Models.Entities
{
    public class Specialist
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(120)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Specialty { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [MaxLength(100)]
        public string? ContactMethod { get; set; }

        [MaxLength(200)]
        public string? Location { get; set; }

        public bool IsVerified { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ICollection<SpecialistContactRequest> SpecialistContactRequests { get; set; } = new List<SpecialistContactRequest>();
    }
}
