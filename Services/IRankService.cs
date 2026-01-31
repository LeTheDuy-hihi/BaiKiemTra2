using PickleballClubManagement.Data;
using PickleballClubManagement.Models;

namespace PickleballClubManagement.Services
{
    public interface IRankService
    {
        Task UpdateRankAfterMatchAsync(Match match);
        double CalculateExpectedScore(double playerRank, double opponentRank);
        double CalculateNewRank(double currentRank, double expectedScore, bool isWin, int kFactor = 32);
    }

    public class RankService : IRankService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RankService> _logger;

        public RankService(ApplicationDbContext context, ILogger<RankService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Tính điểm thắng kỳ vọng dựa trên công thức Elo
        /// </summary>
        public double CalculateExpectedScore(double playerRank, double opponentRank)
        {
            double rankDifference = opponentRank - playerRank;
            double expectedScore = 1.0 / (1.0 + Math.Pow(10, rankDifference / 400.0));
            return expectedScore;
        }

        /// <summary>
        /// Tính rank mới sau trận đấu
        /// </summary>
        public double CalculateNewRank(double currentRank, double expectedScore, bool isWin, int kFactor = 32)
        {
            double actualScore = isWin ? 1.0 : 0.0;
            double newRank = currentRank + kFactor * (actualScore - expectedScore);
            return Math.Round(newRank, 2);
        }

        /// <summary>
        /// Cập nhật rank cho tất cả người chơi sau khi trận đấu kết thúc
        /// </summary>
        public async Task UpdateRankAfterMatchAsync(Match match)
        {
            if (!match.IsRanked)
                return;

            try
            {
                // Lấy thông tin người chơi từ DB
                var winner1 = await _context.Members.FindAsync(match.Winner1Id);
                var loser1 = await _context.Members.FindAsync(match.Loser1Id);

                if (winner1 == null || loser1 == null)
                {
                    _logger.LogWarning($"Match {match.Id}: Không tìm thấy Member");
                    return;
                }

                // K-Factor: 64 cho Challenge Match, 32 cho trận thường
                int kFactor = match.ChallengeId.HasValue ? 64 : 32;

                // Cập nhật rank cho Winner1 và Loser1
                double expectedScoreWinner1 = CalculateExpectedScore(winner1.RankLevel, loser1.RankLevel);
                double newRankWinner1 = CalculateNewRank(winner1.RankLevel, expectedScoreWinner1, true, kFactor);

                double expectedScoreLoser1 = CalculateExpectedScore(loser1.RankLevel, winner1.RankLevel);
                double newRankLoser1 = CalculateNewRank(loser1.RankLevel, expectedScoreLoser1, false, kFactor);

                winner1.RankLevel = newRankWinner1;
                winner1.WinMatches++;
                winner1.TotalMatches++;

                loser1.RankLevel = newRankLoser1;
                loser1.TotalMatches++;

                // Nếu là Doubles, cập nhật rank cho Winner2 và Loser2
                if (match.Format == MatchFormat.Doubles && match.Winner2Id.HasValue && match.Loser2Id.HasValue)
                {
                    var winner2 = await _context.Members.FindAsync(match.Winner2Id);
                    var loser2 = await _context.Members.FindAsync(match.Loser2Id);

                    if (winner2 != null && loser2 != null)
                    {
                        expectedScoreWinner1 = CalculateExpectedScore(winner2.RankLevel, loser2.RankLevel);
                        newRankWinner1 = CalculateNewRank(winner2.RankLevel, expectedScoreWinner1, true, kFactor);

                        expectedScoreLoser1 = CalculateExpectedScore(loser2.RankLevel, winner2.RankLevel);
                        newRankLoser1 = CalculateNewRank(loser2.RankLevel, expectedScoreLoser1, false, kFactor);

                        winner2.RankLevel = newRankWinner1;
                        winner2.WinMatches++;
                        winner2.TotalMatches++;

                        loser2.RankLevel = newRankLoser1;
                        loser2.TotalMatches++;
                    }
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation($"Match {match.Id}: Cập nhật rank thành công");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi cập nhật rank: {ex.Message}");
                throw;
            }
        }
    }
}
