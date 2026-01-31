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

        public async Task<List<Member>> GetTopRankedMembersAsync(int count = 5)
        {
            return await _context.Members
                .Where(m => m.Status == "Active")
                .OrderByDescending(m => m.RankLevel)
                .Take(count)
                .ToListAsync();
        }

        public async Task<Member> UpdateMemberAsync(int memberId, string? fullName, DateTime? dateOfBirth, string? phoneNumber, string? email)
        {
            var member = await _context.Members.FindAsync(memberId);
            if (member != null)
            {
                if (!string.IsNullOrEmpty(fullName))
                    member.FullName = fullName;

                if (dateOfBirth.HasValue)
                    member.DateOfBirth = dateOfBirth;

                if (!string.IsNullOrEmpty(phoneNumber))
                    member.PhoneNumber = phoneNumber;

                if (!string.IsNullOrEmpty(email))
                    member.Email = email;

                await _context.SaveChangesAsync();
            }

            return member!;
        }
    }
}
