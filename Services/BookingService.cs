using Microsoft.EntityFrameworkCore;
using PickleballClubManagement.Data;
using PickleballClubManagement.Models;

namespace PickleballClubManagement.Services
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;

        public BookingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsTimeSlotAvailableAsync(int courtId, DateTime startTime, DateTime endTime)
        {
            // Check for overlap on the specific court
            var hasOverlap = await _context.Bookings.AnyAsync(b =>
                b.CourtId == courtId &&
                b.Status != BookingStatus.Cancelled &&
                startTime < b.EndTime && endTime > b.StartTime);
            
            return !hasOverlap;
        }

        public async Task<Booking> CreateBookingAsync(int memberId, int courtId, DateTime startTime, DateTime endTime, string? notes)
        {
            var booking = new Booking
            {
                MemberId = memberId,
                CourtId = courtId,
                StartTime = startTime,
                EndTime = endTime,
                Status = BookingStatus.Confirmed,
                Notes = notes,
                CreatedAt = DateTime.Now
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return booking;
        }

        public async Task<List<Booking>> GetBookingsByDateAsync(DateTime date)
        {
            var startOfDay = date.Date;
            var endOfDay = startOfDay.AddDays(1);

            return await _context.Bookings
                .Include(b => b.Member)
                .Include(b => b.Court) // Added Court include
                .Where(b => b.StartTime >= startOfDay && b.StartTime < endOfDay && b.Status != BookingStatus.Cancelled) // Added status filter
                .OrderBy(b => b.StartTime)
                .ToListAsync();
        }

        public async Task<List<Booking>> GetMemberBookingsAsync(int memberId)
        {
            return await _context.Bookings
                .Include(b => b.Member)
                .Where(b => b.MemberId == memberId)
                .OrderByDescending(b => b.StartTime)
                .ToListAsync();
        }
    }
}
