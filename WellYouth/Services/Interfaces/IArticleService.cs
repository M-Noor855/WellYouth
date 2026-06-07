using WellYouth.Models.Entities;

namespace WellYouth.Services.Interfaces
{
    public interface IArticleService
    {
        Task<List<Article>> GetPublishedArticlesAsync();
        Task<List<Article>> GetArticlesByCategoryAsync(int categoryId);
        Task<Article?> GetArticleBySlugAsync(string slug);
        Task<List<ArticleCategory>> GetCategoriesAsync();
        Task<List<Article>> SearchArticlesAsync(string query);
    }
}