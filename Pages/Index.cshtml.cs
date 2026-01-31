using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PickleballClubManagement.Data;
using PickleballClubManagement.Models;
using PickleballClubManagement.Services;

namespace PickleballClubManagement.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ITreasuryService _treasuryService;
        private readonly IMemberService _memberService;
        private readonly IBookingService _bookingService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(
            ApplicationDbContext context,
            ITreasuryService treasuryService,
            IMemberService memberService,
            IBookingService bookingService,
            ILogger<IndexModel> logger)
        {
            _context = context;
            _treasuryService = treasuryService;
            _memberService = memberService;
            _bookingService = bookingService;
            _logger = logger;
        }

        public int TotalMembers { get; set; }
        public int ActiveChallenges { get; set; }
        public int TotalMatches { get; set; }
        public int AvailableCourts { get; set; }
        public List<News> FeaturedNews { get; set; } = new();
        public List<Member> TopMembers { get; set; } = new();

        // Treasury Info
        public decimal TreasuryBalance { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public bool IsTreasuryDeficit { get; set; }
        public List<Booking> UpcomingBookings { get; set; } = new();

        public async Task OnGetAsync()
        {
            try
            {
                TotalMembers = await _context.Members.CountAsync();
                ActiveChallenges = await _context.Challenges.CountAsync(c => c.Status == ChallengeStatus.Open || c.Status == ChallengeStatus.Ongoing);
                TotalMatches = await _context.Matches.CountAsync();
                AvailableCourts = await _context.Courts.CountAsync(c => c.IsActive);

                // Lấy tin tức nổi bật (tin tức mới nhất, có pin hoặc mới)
                FeaturedNews = await _context.News
                    .OrderByDescending(n => n.IsPinned)
                    .ThenByDescending(n => n.CreatedDate)
                    .Take(3)
                    .ToListAsync();

                // Lấy top 5 thành viên theo điểm ranking
                TopMembers = await _context.Members
                    .OrderByDescending(m => m.RankLevel)
                    .Take(5)
                    .ToListAsync();

                // Treasury Info
                TreasuryBalance = await _treasuryService.GetTreasuryBalanceAsync();
                TotalIncome = await _treasuryService.GetTotalIncomeAsync();
                TotalExpense = await _treasuryService.GetTotalExpenseAsync();
                IsTreasuryDeficit = await _treasuryService.IsTreasuryDeficitAsync();

                // Upcoming Bookings (next 7 days)
                var today = DateTime.Now;
                var bookingsToday = await _bookingService.GetBookingsByDateAsync(today);
                UpcomingBookings = bookingsToday
                    .Where(b => b.StartTime >= today)
                    .OrderBy(b => b.StartTime)
                    .Take(5)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi khi tải Dashboard: {ex.Message}");
            }
        }
    }
}
