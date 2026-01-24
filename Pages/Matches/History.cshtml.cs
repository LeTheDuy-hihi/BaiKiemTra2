using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PickleballClubManagement.Models;
using PickleballClubManagement.Services;

namespace PickleballClubManagement.Pages.Matches
{
    [Authorize]
    public class HistoryModel : PageModel
    {
        private readonly IMatchService _matchService;

        public HistoryModel(IMatchService matchService)
        {
            _matchService = matchService;
        }

        public List<Match> Matches { get; set; } = new();

        public async Task OnGetAsync()
        {
            Matches = await _matchService.GetMatchHistoryAsync();
        }
    }
}
