using WellYouth.Models.Entities;

namespace WellYouth.Services.Interfaces
{
    public interface ICommunityService
    {
        Task<List<CommunityGroup>> GetAllGroupsAsync();
        Task<CommunityGroup?> GetGroupByIdAsync(int id);
        Task<List<CommunityPost>> GetGroupPostsAsync(int groupId);
        Task<bool> CreatePostAsync(CommunityPost post);
        Task<bool> JoinGroupAsync(int groupId, string userId);
        Task<bool> IsMemberAsync(int groupId, string userId);
        Task<List<CommunityGroup>> GetUserGroupsAsync(string userId);
        Task<bool> DeletePostAsync(int postId, string userId);
        Task<bool> LeaveGroupAsync(int groupId, string userId);
    }
}