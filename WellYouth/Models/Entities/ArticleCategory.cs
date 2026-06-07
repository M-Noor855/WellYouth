using System.ComponentModel.DataAnnotations;

namespace WellYouth.Models.Entities
{
    public class ArticleCategory
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(60)]
        public string Slug { get; set; } = string.Empty;

        // Navigation properties
        public ICollection<Article> Articles { get; set; } = new List<Article>();
    }
}
