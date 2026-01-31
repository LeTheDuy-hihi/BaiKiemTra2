using Microsoft.EntityFrameworkCore;
using PickleballClubManagement.Data;
using PickleballClubManagement.Models;

namespace PickleballClubManagement.Services
{
    public class MatchService : IMatchService
    {
        private readonly ApplicationDbContext _context;
        private readonly IRankService _rankService;

        public MatchService(ApplicationDbContext context, IRankService rankService)
        {
            _context = context;
            _rankService = rankService;
        }

        public async Task<Match> CreateMatchAsync(int? challengeId, MatchFormat format, int winner1Id, int? winner2Id, int loser1Id, int? loser2Id, bool isRanked)
        {
            // Validation
            if (format == MatchFormat.Doubles && (!winner2Id.HasValue || !loser2Id.HasValue))
                throw new ArgumentException("Doubles match phải có đủ 4 người chơi");

            if (winner1Id == loser1Id || (winner2Id.HasValue && winner2Id == loser2Id))
                throw new ArgumentException("Không được chọn cùng một người hai lần");

            // Create match
            var match = new Match
            {
                ChallengeId = challengeId,
                Format = format,
                Winner1Id = winner1Id,
                Winner2Id = winner2Id,
                Loser1Id = loser1Id,
                Loser2Id = loser2Id,
                IsRanked = isRanked,
                MatchDate = DateTime.Now,
                WinningSide = WinningSide.Team1
            };

            _context.Matches.Add(match);
            await _context.SaveChangesAsync();

            // Update rankings if match is ranked
            if (isRanked)
            {
                await _rankService.UpdateRankAfterMatchAsync(match);
            }

            // If match is linked to a challenge, update challenge
            if (challengeId.HasValue)
            {
                var challenge = await _context.Challenges.FindAsync(challengeId.Value);
                if (challenge != null)
                {
                    if (challenge.ResultMode == GameMode.TeamBattle)
                    {
                        challenge.CurrentScore_TeamA++;
                        if (challenge.CurrentScore_TeamA >= challenge.Config_TargetWins)
                        {
                            challenge.Status = ChallengeStatus.Finished;
                            challenge.EndDate = DateTime.Now;
                        }
                    }
                    await _context.SaveChangesAsync();
                }
            }

            return match;
        }

        public async Task<List<Match>> GetMatchHistoryAsync(int? memberId = null)
        {
            var query = _context.Matches
                .Include(m => m.Challenge)
                .Include(m => m.Winner1)
                .Include(m => m.Winner2)
                .Include(m => m.Loser1)
                .Include(m => m.Loser2)
                .OrderByDescending(m => m.MatchDate)
                .AsQueryable();

            if (memberId.HasValue)
            {
                query = query.Where(m =>
                    m.Winner1Id == memberId ||
                    m.Winner2Id == memberId ||
                    m.Loser1Id == memberId ||
                    m.Loser2Id == memberId);
            }

            return await query.ToListAsync();
        }

        public async Task<Match?> GetMatchByIdAsync(int id)
        {
            return await _context.Matches
                .Include(m => m.Challenge)
                .Include(m => m.Winner1)
                .Include(m => m.Winner2)
                .Include(m => m.Loser1)
                .Include(m => m.Loser2)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task UpdateMatchAsync(int matchId, WinningSide winningSide, bool isRanked)
        {
            var match = await GetMatchByIdAsync(matchId);
            if (match == null)
                throw new ArgumentException("Match không tồn tại");

            match.WinningSide = winningSide;
            match.IsRanked = isRanked;

            if (isRanked)
            {
                await _rankService.UpdateRankAfterMatchAsync(match);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteMatchAsync(int matchId)
        {
            var match = await _context.Matches.FindAsync(matchId);
            if (match != null)
            {
                _context.Matches.Remove(match);
                await _context.SaveChangesAsync();
            }
        }
    }
}
