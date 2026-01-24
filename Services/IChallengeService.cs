using PickleballClubManagement.Models;

namespace PickleballClubManagement.Services
{
    public interface IChallengeService
    {
        Task<List<Challenge>> GetOpenChallengesAsync();
        Task<List<Challenge>> GetAcceptedChallengesAsync();
        Task<List<Challenge>> GetAllChallengesAsync();
        Task<Challenge> CreateChallengeAsync(int creatorId, string title, string description);
        Task<Challenge> CreateAdvancedChallengeAsync(int creatorId, string title, string description, ChallengeType type, GameMode mode, decimal entryFee, decimal prizePool, int targetWins);
        Task<bool> AcceptChallengeAsync(int challengeId);
        Task<Challenge?> GetChallengeByIdAsync(int id);
    }
}
