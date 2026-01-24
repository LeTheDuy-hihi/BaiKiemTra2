using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PickleballClubManagement.Models;
using PickleballClubManagement.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace PickleballClubManagement.Pages.Bookings
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IBookingService _bookingService;
        private readonly IMemberService _memberService;
        private readonly PickleballClubManagement.Data.ApplicationDbContext _context;

        public CreateModel(
            IBookingService bookingService, 
            IMemberService memberService,
            PickleballClubManagement.Data.ApplicationDbContext context)
        {
            _bookingService = bookingService;
            _memberService = memberService;
            _context = context;
        }

        [BindProperty]
        public BookingInputModel Input { get; set; } = new();

        public List<Booking> TodayBookings { get; set; } = new();
        public List<Court> Courts { get; set; } = new();

        public class BookingInputModel
        {
            [Required]
            public int CourtId { get; set; }

            [Required]
            public DateTime Date { get; set; } = DateTime.Now;

            [Required]
            public TimeSpan StartTime { get; set; }

            [Required]
            public TimeSpan EndTime { get; set; }
            
            public string? Notes { get; set; }
        }

        public async Task OnGetAsync()
        {
            Courts = await _context.Courts.Where(c => c.IsActive).ToListAsync();
            TodayBookings = await _bookingService.GetBookingsByDateAsync(DateTime.Now);
            
            // Set default times
            Input.Date = DateTime.Now;
            Input.StartTime = new TimeSpan(DateTime.Now.Hour + 1, 0, 0);
            Input.EndTime = new TimeSpan(DateTime.Now.Hour + 2, 0, 0);
            
            if (Courts.Any())
            {
                Input.CourtId = Courts.First().Id;
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Courts = await _context.Courts.Where(c => c.IsActive).ToListAsync();
            TodayBookings = await _bookingService.GetBookingsByDateAsync(Input.Date);
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Account/Login");
            }

            var member = await _memberService.GetMemberByUserIdAsync(userId);

            if (!ModelState.IsValid || member == null)
            {
                return Page();
            }

            var startDateTime = Input.Date.Date + Input.StartTime;
            var endDateTime = Input.Date.Date + Input.EndTime;

            if (startDateTime < DateTime.Now)
            {
                TempData["ErrorMessage"] = "⚠️ Không thể đặt giờ trong quá khứ!";
                return Page();
            }

            if (endDateTime <= startDateTime)
            {
                TempData["ErrorMessage"] = "⚠️ Giờ kết thúc phải sau giờ bắt đầu!";
                return Page();
            }

            // Check overlap with CourtId
            var isAvailable = await _bookingService.IsTimeSlotAvailableAsync(Input.CourtId, startDateTime, endDateTime);
            if (!isAvailable)
            {
                TempData["ErrorMessage"] = "⚠️ Sân này đã có người đặt trong khung giờ đó!";
                return Page();
            }

            await _bookingService.CreateBookingAsync(member.Id, Input.CourtId, startDateTime, endDateTime, Input.Notes);

            TempData["SuccessMessage"] = "✅ Đặt sân thành công!";
            return RedirectToPage("/Bookings/Create");
        }
    }
}
