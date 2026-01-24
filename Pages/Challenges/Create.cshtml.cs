using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PickleballClubManagement.Models;
using PickleballClubManagement.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace PickleballClubManagement.Pages.Challenges
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IChallengeService _challengeService;
        private readonly IMemberService _memberService;

        public CreateModel(IChallengeService challengeService, IMemberService memberService)
        {
            _challengeService = challengeService;
            _memberService = memberService;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required(ErrorMessage = "Tiêu đề là bắt buộc")]
            [StringLength(100, ErrorMessage = "Tiêu đề không quá 100 ký tự")]
            public string Title { get; set; } = string.Empty;

            [Required(ErrorMessage = "Mô tả là bắt buộc")]
            [StringLength(500, ErrorMessage = "Mô tả không quá 500 ký tự")]
            public string Description { get; set; } = string.Empty;
            
            // Advanced Fields
            public ChallengeType Type { get; set; } = ChallengeType.Duel;
            public GameMode GameMode { get; set; } = GameMode.TeamBattle;
            public decimal EntryFee { get; set; }
            public decimal PrizePool { get; set; }
            public int TargetWins { get; set; } = 3;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Account/Login");
            }

            var member = await _memberService.GetMemberByUserIdAsync(userId);

            if (member == null)
            {
                return RedirectToPage("/Account/Login");
            }

            if (Input.Type == ChallengeType.Duel)
            {
                await _challengeService.CreateChallengeAsync(member.Id, Input.Title, Input.Description);
            }
            else
            {
                await _challengeService.CreateAdvancedChallengeAsync(
                    member.Id, 
                    Input.Title, 
                    Input.Description, 
                    Input.Type, 
                    Input.GameMode, 
                    Input.EntryFee, 
                    Input.PrizePool, 
                    Input.TargetWins
                );
            }

            TempData["SuccessMessage"] = "Tạo kèo thành công!";
            return RedirectToPage("/Challenges/Index");
        }
    }
}
