using PickleballClubManagement.Models;

namespace PickleballClubManagement.Services
{
    public interface IMemberService
    {
        Task<Member?> GetMemberByUserIdAsync(string userId);
        Task<Member?> GetMemberByIdAsync(int id);
        Task<Member> CreateMemberAsync(string userId, string fullName);
        Task<List<Member>> GetAllMembersAsync();
        Task<List<Member>> GetTopRankedMembersAsync(int count = 5);
        Task<Member> UpdateMemberAsync(int memberId, string? fullName, DateTime? dateOfBirth, string? phoneNumber, string? email);
    }
}
