using Microsoft.EntityFrameworkCore;
using PickleballClubManagement.Data;
using PickleballClubManagement.Models;

namespace PickleballClubManagement.Services
{
    public class MatchService : IMatchService
    {
        private readonly ApplicationDbContext _context;

        public MatchService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Match> CreateMatchAsync(int? challengeId, MatchFormat format, int winner1Id, int? winner2Id, int loser1Id, int? loser2Id, bool isRanked)
        {
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
                MatchDate = DateTime.Now
            };

            _context.Matches.Add(match);
            await _context.SaveChangesAsync();

            // If match is linked to a challenge, update challenge status to Completed
            if (challengeId.HasValue)
            {
                var challenge = await _context.Challenges.FindAsync(challengeId.Value);
                if (challenge != null)
                {
                    challenge.Status = ChallengeStatus.Completed;
                    await _context.SaveChangesAsync();
                }
            }

            // Update rankings if match is ranked
            if (isRanked)
            {
                await UpdateRankingsAsync(match);
            }

            return match;
        }

        private async Task UpdateRankingsAsync(Match match)
        {
            // Add 0.1 to winners
            await UpdateMemberRankAsync(match.Winner1Id, 0.1);
            if (match.Winner2Id.HasValue)
            {
                await UpdateMemberRankAsync(match.Winner2Id.Value, 0.1);
            }

            // Subtract 0.1 from losers (minimum 1.0)
            await UpdateMemberRankAsync(match.Loser1Id, -0.1);
            if (match.Loser2Id.HasValue)
            {
                await UpdateMemberRankAsync(match.Loser2Id.Value, -0.1);
            }
        }

        private async Task UpdateMemberRankAsync(int memberId, double delta)
        {
            var member = await _context.Members.FindAsync(memberId);
            if (member != null)
            {
                member.RankLevel = Math.Max(1.0, member.RankLevel + delta);
                await _context.SaveChangesAsync();
            }
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
    }
}
