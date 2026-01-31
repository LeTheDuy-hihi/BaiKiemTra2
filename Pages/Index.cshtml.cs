using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PickleballClubManagement.Data;
using PickleballClubManagement.Models;

namespace PickleballClubManagement.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public int TotalMembers { get; set; }
        public int ActiveChallenges { get; set; }
        public int TotalMatches { get; set; }
        public int AvailableCourts { get; set; }
        public List<News> FeaturedNews { get; set; } = new();
        public List<Member> TopMembers { get; set; } = new();

        public async Task OnGetAsync()
        {
            TotalMembers = await _context.Members.CountAsync();
            ActiveChallenges = await _context.Challenges.CountAsync(c => c.Status == Models.ChallengeStatus.Open);
            TotalMatches = await _context.Matches.CountAsync();
            AvailableCourts = await _context.Courts.CountAsync(c => c.IsActive);

            // Lấy tin tức nổi bật (tin tức mới nhất, có pin hoặc mới)
            FeaturedNews = await _context.News
                .OrderByDescending(n => n.IsPinned)
                .ThenByDescending(n => n.CreatedDate)
                .Take(3)
                .ToListAsync();

            // Lấy top 5 thành viên theo điểm ranking
            TopMembers = await _context.Members
                .OrderByDescending(m => m.RankLevel)
                .Take(5)
                .ToListAsync();
        }
    }
}
