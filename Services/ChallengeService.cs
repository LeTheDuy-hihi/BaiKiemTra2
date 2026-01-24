using Microsoft.EntityFrameworkCore;
using PickleballClubManagement.Data;
using PickleballClubManagement.Models;

namespace PickleballClubManagement.Services
{
    public class ChallengeService : IChallengeService
    {
        private readonly ApplicationDbContext _context;

        public ChallengeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Challenge>> GetOpenChallengesAsync()
        {
            return await _context.Challenges
                .Include(c => c.Creator)
                .Where(c => c.Status == ChallengeStatus.Open)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Challenge>> GetAcceptedChallengesAsync()
        {
            return await _context.Challenges
                .Include(c => c.Creator)
                .Where(c => c.Status == ChallengeStatus.Accepted)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Challenge>> GetAllChallengesAsync()
        {
            return await _context.Challenges
                .Include(c => c.Creator)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<Challenge> CreateChallengeAsync(int creatorId, string title, string description)
        {
            var challenge = new Challenge
            {
                CreatorId = creatorId,
                Title = title,
                Description = description,
                Status = ChallengeStatus.Open,
                Type = ChallengeType.Duel,
                CreatedAt = DateTime.Now
            };

            _context.Challenges.Add(challenge);
            await _context.SaveChangesAsync();

            return challenge;
        }

        public async Task<Challenge> CreateAdvancedChallengeAsync(int creatorId, string title, string description, ChallengeType type, GameMode mode, decimal entryFee, decimal prizePool, int targetWins)
        {
            var challenge = new Challenge
            {
                CreatorId = creatorId,
                Title = title,
                Description = description,
                Status = type == ChallengeType.MiniGame ? ChallengeStatus.Ongoing : ChallengeStatus.Open, // Mini-games start as Ongoing to accept participants
                Type = type,
                ResultMode = mode,
                EntryFee = entryFee,
                PrizePool = prizePool,
                Config_TargetWins = targetWins,
                StartDate = DateTime.Now,
                CreatedAt = DateTime.Now
            };

            _context.Challenges.Add(challenge);
            await _context.SaveChangesAsync();

            return challenge;
        }

        public async Task<bool> AcceptChallengeAsync(int challengeId)
        {
            var challenge = await _context.Challenges.FindAsync(challengeId);
            if (challenge == null || challenge.Status != ChallengeStatus.Open)
            {
                return false;
            }

            challenge.Status = ChallengeStatus.Accepted;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Challenge?> GetChallengeByIdAsync(int id)
        {
            return await _context.Challenges
                .Include(c => c.Creator)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
