using Microsoft.EntityFrameworkCore;
using WellYouth.Data;
using WellYouth.Models.Entities;
using WellYouth.Services.Interfaces;

namespace WellYouth.Services.Implementations
{
    public class SpecialistService : ISpecialistService
    {
        private readonly ApplicationDbContext _context;

        public SpecialistService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Specialist>> GetAllSpecialistsAsync()
        {
            return await _context.Specialists
                .OrderByDescending(s => s.IsVerified)
                .ThenBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<List<Specialist>> GetSpecialistsBySpecialtyAsync(string specialty)
        {
            return await _context.Specialists
                .Where(s => s.Specialty == specialty)
                .OrderByDescending(s => s.IsVerified)
                .ThenBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<Specialist?> GetSpecialistByIdAsync(int id)
        {
            return await _context.Specialists.FindAsync(id);
        }

        public async Task<bool> SubmitContactRequestAsync(SpecialistContactRequest request)
        {
            _context.SpecialistContactRequests.Add(request);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<string>> GetSpecialtiesAsync()
        {
            return await _context.Specialists
                .Select(s => s.Specialty)
                .Distinct()
                .OrderBy(s => s)
                .ToListAsync();
        }
    }
}