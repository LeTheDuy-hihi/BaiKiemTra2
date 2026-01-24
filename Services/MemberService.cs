using Microsoft.EntityFrameworkCore;
using PickleballClubManagement.Data;
using PickleballClubManagement.Models;

namespace PickleballClubManagement.Services
{
    public class MemberService : IMemberService
    {
        private readonly ApplicationDbContext _context;

        public MemberService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Member?> GetMemberByUserIdAsync(string userId)
        {
            return await _context.Members
                .FirstOrDefaultAsync(m => m.IdentityUserId == userId);
        }

        public async Task<Member?> GetMemberByIdAsync(int id)
        {
            return await _context.Members.FindAsync(id);
        }

        public async Task<Member> CreateMemberAsync(string userId, string fullName)
        {
            var member = new Member
            {
                IdentityUserId = userId,
                FullName = fullName,
                Status = "Active",
                RankLevel = 1.0
            };

            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            return member;
        }

        public async Task<List<Member>> GetAllMembersAsync()
        {
            return await _context.Members
                .Where(m => m.Status == "Active")
                .OrderBy(m => m.FullName)
                .ToListAsync();
        }
    }
}
