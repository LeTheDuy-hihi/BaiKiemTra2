using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PickleballClubManagement.Models;
using PickleballClubManagement.Services;

namespace PickleballClubManagement.Pages.Challenges
{
    [Authorize(Roles = "Admin")]
    public class ManageModel : PageModel
    {
        private readonly IChallengeService _challengeService;

        public ManageModel(IChallengeService challengeService)
        {
            _challengeService = challengeService;
        }

        public List<Challenge> Challenges { get; set; } = new();
        public List<Challenge> AllChallenges { get; set; } = new();
        public ChallengeStatus? FilterStatus { get; set; }

        public async Task OnGetAsync(int? status)
        {
            // Get all challenges for counting
            AllChallenges = await _challengeService.GetAllChallengesAsync();

            if (status.HasValue && Enum.IsDefined(typeof(ChallengeStatus), status.Value))
            {
                FilterStatus = (ChallengeStatus)status.Value;
                Challenges = AllChallenges.Where(c => c.Status == FilterStatus.Value).ToList();
            }
            else
            {
                Challenges = AllChallenges;
            }
        }
    }
}
