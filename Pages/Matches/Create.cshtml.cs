using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PickleballClubManagement.Models;
using PickleballClubManagement.Services;
using System.ComponentModel.DataAnnotations;

namespace PickleballClubManagement.Pages.Matches
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly IMatchService _matchService;
        private readonly IChallengeService _challengeService;
        private readonly IMemberService _memberService;

        public CreateModel(IMatchService matchService, IChallengeService challengeService, IMemberService memberService)
        {
            _matchService = matchService;
            _challengeService = challengeService;
            _memberService = memberService;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public List<Challenge> AcceptedChallenges { get; set; } = new();
        public List<Member> Members { get; set; } = new();

        public class InputModel
        {
            [Required]
            public MatchFormat Format { get; set; } = MatchFormat.Singles;

            public int? ChallengeId { get; set; }

            [Required(ErrorMessage = "Vui lòng chọn người thắng 1")]
            public int Winner1Id { get; set; }

            public int? Winner2Id { get; set; }

            [Required(ErrorMessage = "Vui lòng chọn người thua 1")]
            public int Loser1Id { get; set; }

            public int? Loser2Id { get; set; }

            public bool IsRanked { get; set; } = true;
        }

        public async Task OnGetAsync()
        {
            AcceptedChallenges = await _challengeService.GetAcceptedChallengesAsync();
            Members = await _memberService.GetAllMembersAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Reload data for validation
            AcceptedChallenges = await _challengeService.GetAcceptedChallengesAsync();
            Members = await _memberService.GetAllMembersAsync();

            // Validate format-specific requirements
            if (Input.Format == MatchFormat.Singles)
            {
                Input.Winner2Id = null;
                Input.Loser2Id = null;
            }
            else // Doubles
            {
                if (!Input.Winner2Id.HasValue)
                {
                    ModelState.AddModelError("Input.Winner2Id", "Vui lòng chọn người thắng 2 cho trận Đôi");
                }
                if (!Input.Loser2Id.HasValue)
                {
                    ModelState.AddModelError("Input.Loser2Id", "Vui lòng chọn người thua 2 cho trận Đôi");
                }
            }

            // Validate no duplicate players
            var players = new List<int> { Input.Winner1Id, Input.Loser1Id };
            if (Input.Winner2Id.HasValue) players.Add(Input.Winner2Id.Value);
            if (Input.Loser2Id.HasValue) players.Add(Input.Loser2Id.Value);

            if (players.Count != players.Distinct().Count())
            {
                ModelState.AddModelError("", "Một người không thể vừa thắng vừa thua, hoặc xuất hiện nhiều lần!");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Create match
            await _matchService.CreateMatchAsync(
                Input.ChallengeId,
                Input.Format,
                Input.Winner1Id,
                Input.Winner2Id,
                Input.Loser1Id,
                Input.Loser2Id,
                Input.IsRanked
            );

            TempData["SuccessMessage"] = "Ghi nhận kết quả thành công!";
            return RedirectToPage("/Matches/History");
        }
    }
}
