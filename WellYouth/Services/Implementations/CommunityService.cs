using Microsoft.EntityFrameworkCore;
using WellYouth.Data;
using WellYouth.Models.Entities;
using WellYouth.Services.Interfaces;

namespace WellYouth.Services.Implementations
{
    public class CommunityService : ICommunityService
    {
        private readonly ApplicationDbContext _context;

        public CommunityService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CommunityGroup>> GetAllGroupsAsync()
        {
            return await _context.CommunityGroups
                .OrderByDescending(g => g.CreatedAt)
                .ToListAsync();
        }

        public async Task<CommunityGroup?> GetGroupByIdAsync(int id)
        {
            return await _context.CommunityGroups.FindAsync(id);
        }

        public async Task<List<CommunityPost>> GetGroupPostsAsync(int groupId)
        {
            return await _context.CommunityPosts
                .Include(p => p.User)
                .Where(p => p.GroupId == groupId && p.Status == "Approved")
                .OrderByDescending(p => p.PostTime)
                .ToListAsync();
        }

        public async Task<bool> CreatePostAsync(CommunityPost post)
        {
            _context.CommunityPosts.Add(post);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> JoinGroupAsync(int groupId, string userId)
        {
            if (await IsMemberAsync(groupId, userId)) return true;

            var membership = new CommunityGroupMember
            {
                GroupId = groupId,
                UserId = userId,
                JoinedAt = DateTime.UtcNow
            };
            _context.CommunityGroupMembers.Add(membership);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsMemberAsync(int groupId, string userId)
        {
            return await _context.CommunityGroupMembers
                .AnyAsync(m => m.GroupId == groupId && m.UserId == userId);
        }

        public async Task<List<CommunityGroup>> GetUserGroupsAsync(string userId)
        {
            return await _context.CommunityGroupMembers
                .Where(m => m.UserId == userId && m.Group != null)
                .Select(m => m.Group!)
                .ToListAsync();
        }
    }
}