using WellYouth.Models.Entities;

namespace WellYouth.Services.Interfaces
{
    public interface IRecommendationService
    {
        Task<List<string>> GetHealthSuggestionsAsync(string userId);
    }
}