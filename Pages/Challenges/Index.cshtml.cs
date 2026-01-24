using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PickleballClubManagement.Data;
using PickleballClubManagement.Models;
using PickleballClubManagement.Services;
using System.Security.Claims;

namespace PickleballClubManagement.Pages.Challenges
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context; // Use context directly for complex query or add new Service method
        private readonly IChallengeService _challengeService;
        private readonly IMemberService _memberService;

        public IndexModel(IChallengeService challengeService, IMemberService memberService, ApplicationDbContext context)
        {
            _challengeService = challengeService;
            _memberService = memberService;
            _context = context;
        }

        public List<Challenge> Challenges { get; set; } = new();
        public int? CurrentMemberId { get; set; }

        public async Task OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userId))
            {
                var member = await _memberService.GetMemberByUserIdAsync(userId);
                CurrentMemberId = member?.Id;
            }

            // Get Open Duels OR Ongoing MiniGames
            Challenges = await _context.Challenges
                .Include(c => c.Creator)
                .Where(c => c.Status == ChallengeStatus.Open || c.Status == ChallengeStatus.Ongoing)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAcceptAsync(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Account/Login");
            }
            
            var member = await _memberService.GetMemberByUserIdAsync(userId);

            if (member == null) return RedirectToPage("/Account/Login");

            var result = await _challengeService.AcceptChallengeAsync(id);

            if (result)
            {
                TempData["SuccessMessage"] = "Đã nhận kèo thành công! Hãy chuẩn bị thi đấu.";
            }
            else
            {
                TempData["ErrorMessage"] = "Không thể nhận kèo này (đã có người nhận hoặc bị xóa).";
            }

            return RedirectToPage();
        }
    }
}
