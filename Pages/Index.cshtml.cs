using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PickleballClubManagement.Data;

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

        public async Task OnGetAsync()
        {
            TotalMembers = await _context.Members.CountAsync();
            ActiveChallenges = await _context.Challenges.CountAsync(c => c.Status == Models.ChallengeStatus.Open);
            TotalMatches = await _context.Matches.CountAsync();
            AvailableCourts = await _context.Courts.CountAsync(c => c.IsActive);
        }
    }
}
