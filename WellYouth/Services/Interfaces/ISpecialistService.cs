using WellYouth.Models.Entities;

namespace WellYouth.Services.Interfaces
{
    public interface ISpecialistService
    {
        Task<List<Specialist>> GetAllSpecialistsAsync();
        Task<List<Specialist>> GetSpecialistsBySpecialtyAsync(string specialty);
        Task<Specialist?> GetSpecialistByIdAsync(int id);
        Task<bool> SubmitContactRequestAsync(SpecialistContactRequest request);
        Task<List<string>> GetSpecialtiesAsync();
    }
}