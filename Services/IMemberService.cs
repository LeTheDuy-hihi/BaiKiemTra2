using PickleballClubManagement.Models;

namespace PickleballClubManagement.Services
{
    public interface IMemberService
    {
        Task<Member?> GetMemberByUserIdAsync(string userId);
        Task<Member?> GetMemberByIdAsync(int id);
        Task<Member> CreateMemberAsync(string userId, string fullName);
        Task<List<Member>> GetAllMembersAsync();
    }
}
