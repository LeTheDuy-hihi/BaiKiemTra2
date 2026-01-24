using PickleballClubManagement.Models;

namespace PickleballClubManagement.Services
{
    public interface IMatchService
    {
        Task<Match> CreateMatchAsync(int? challengeId, MatchFormat format, int winner1Id, int? winner2Id, int loser1Id, int? loser2Id, bool isRanked);
        Task<List<Match>> GetMatchHistoryAsync(int? memberId = null);
        Task<Match?> GetMatchByIdAsync(int id);
    }
}
