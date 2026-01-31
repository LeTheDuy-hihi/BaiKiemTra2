using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PickleballClubManagement.Models;
using PickleballClubManagement.Services;
using System.Security.Claims;

namespace PickleballClubManagement.Pages.Bookings
{
    [Authorize]
    public class HistoryModel : PageModel
    {
        private readonly IBookingService _bookingService;
        private readonly IMemberService _memberService;

        public HistoryModel(
            IBookingService bookingService,
            IMemberService memberService)
        {
            _bookingService = bookingService;
            _memberService = memberService;
        }

        public List<Booking> Bookings { get; set; } = new();
        public Member? CurrentMember { get; set; }
        public string FilterStatus { get; set; } = "all";

        public async Task OnGetAsync(string? status)
        {
            // Get current user's member ID
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return;

            CurrentMember = await _memberService.GetMemberByUserIdAsync(userId);
            if (CurrentMember == null)
                return;

            // Get member's bookings
            var allBookings = await _bookingService.GetMemberBookingsAsync(CurrentMember.Id);

            // Filter by status if provided
            FilterStatus = status ?? "all";
            if (FilterStatus != "all" && Enum.TryParse<BookingStatus>(FilterStatus, true, out var statusEnum))
            {
                Bookings = allBookings.Where(b => b.Status == statusEnum).OrderByDescending(b => b.StartTime).ToList();
            }
            else
            {
                Bookings = allBookings.OrderByDescending(b => b.StartTime).ToList();
            }
        }
    }
}
