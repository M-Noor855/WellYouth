using Microsoft.AspNetCore.Mvc;
using WellYouth.Services.Interfaces;

namespace WellYouth.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly IArticleService _articleService;

        public ArticlesController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        public async Task<IActionResult> Index(string? query, int? categoryId)
        {
            var articles = string.IsNullOrEmpty(query) 
                ? (categoryId.HasValue ? await _articleService.GetArticlesByCategoryAsync(categoryId.Value) : await _articleService.GetPublishedArticlesAsync())
                : await _articleService.SearchArticlesAsync(query);

            ViewBag.Categories = await _articleService.GetCategoriesAsync();
            ViewBag.CurrentCategory = categoryId;
            ViewBag.SearchQuery = query;

            return View(articles);
        }

        public async Task<IActionResult> Details(string slug)
        {
            var article = await _articleService.GetArticleBySlugAsync(slug);
            if (article == null) return NotFound();

            return View(article);
        }
    }
}