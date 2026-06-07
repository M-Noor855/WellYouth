using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WellYouth.Models.Entities
{
    public class Article
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(160)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(180)]
        public string Slug { get; set; } = string.Empty;

        [Required]
        [MaxLength(300)]
        public string Summary { get; set; } = string.Empty;

        [Required]
        public string Body { get; set; } = string.Empty;

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public ArticleCategory? Category { get; set; }

        [MaxLength(100)]
        public string? AuthorName { get; set; }

        [MaxLength(100)]
        public string? ReviewedBy { get; set; }

        [MaxLength(150)]
        public string? Source { get; set; }

        [MaxLength(500)]
        public string? SourceUrl { get; set; }

        public DateTime? PublishedDate { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
