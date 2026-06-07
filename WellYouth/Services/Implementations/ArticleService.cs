using Microsoft.EntityFrameworkCore;
using WellYouth.Data;
using WellYouth.Models.Entities;
using WellYouth.Services.Interfaces;

namespace WellYouth.Services.Implementations
{
    public class ArticleService : IArticleService
    {
        private readonly ApplicationDbContext _context;

        public ArticleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Article>> GetPublishedArticlesAsync()
        {
            return await _context.Articles
                .Include(a => a.Category)
                .Where(a => a.Status == "Published")
                .OrderByDescending(a => a.PublishedDate)
                .ToListAsync();
        }

        public async Task<List<Article>> GetArticlesByCategoryAsync(int categoryId)
        {
            return await _context.Articles
                .Include(a => a.Category)
                .Where(a => a.CategoryId == categoryId && a.Status == "Published")
                .OrderByDescending(a => a.PublishedDate)
                .ToListAsync();
        }

        public async Task<Article?> GetArticleBySlugAsync(string slug)
        {
            return await _context.Articles
                .Include(a => a.Category)
                .FirstOrDefaultAsync(a => a.Slug == slug);
        }

        public async Task<List<ArticleCategory>> GetCategoriesAsync()
        {
            return await _context.ArticleCategories.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<List<Article>> SearchArticlesAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return await GetPublishedArticlesAsync();
            
            return await _context.Articles
                .Include(a => a.Category)
                .Where(a => a.Status == "Published" && (a.Title.Contains(query) || a.Summary.Contains(query)))
                .OrderByDescending(a => a.PublishedDate)
                .ToListAsync();
        }
    }
}