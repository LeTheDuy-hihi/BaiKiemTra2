using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PickleballClubManagement.Models;
using PickleballClubManagement.Services;
using System.Security.Claims;

namespace PickleballClubManagement.Pages.Profile
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IMemberService _memberService;
        private readonly IMatchService _matchService;

        public IndexModel(IMemberService memberService, IMatchService matchService)
        {
            _memberService = memberService;
            _matchService = matchService;
        }

        public Member? Member { get; set; }
        public List<Match> RecentMatches { get; set; } = new();
        public int TotalMatches { get; set; }
        public int WinCount { get; set; }
        public int LoseCount { get; set; }
        public double WinRate { get; set; }

        public async Task OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                Member = await _memberService.GetMemberByUserIdAsync(userId);
                
                if (Member != null)
                {
                    RecentMatches = await _matchService.GetMatchHistoryAsync(Member.Id);
                    TotalMatches = RecentMatches.Count;
                    
                    WinCount = RecentMatches.Count(m => 
                        m.Winner1Id == Member.Id || m.Winner2Id == Member.Id);
                    
                    LoseCount = RecentMatches.Count(m => 
                        m.Loser1Id == Member.Id || m.Loser2Id == Member.Id);
                    
                    WinRate = TotalMatches > 0 ? (double)WinCount / TotalMatches * 100 : 0;
                }
            }
        }

        public bool IsWinner(Match match)
        {
            if (Member == null) return false;
            return match.Winner1Id == Member.Id || match.Winner2Id == Member.Id;
        }
    }
}
