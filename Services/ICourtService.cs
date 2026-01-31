using Microsoft.EntityFrameworkCore;
using PickleballClubManagement.Data;
using PickleballClubManagement.Models;

namespace PickleballClubManagement.Services
{
    public interface ICourtService
    {
        Task<List<Court>> GetAllCourtsAsync();
        Task<Court?> GetCourtByIdAsync(int courtId);
        Task<Court> CreateCourtAsync(string name, string? description = null);
        Task UpdateCourtAsync(int courtId, string name, bool isActive, string? description);
        Task DeleteCourtAsync(int courtId);
        Task<bool> IsTimeSlotAvailableAsync(int courtId, DateTime startTime, DateTime endTime);
        Task<List<Booking>> GetCourtBookingsAsync(int courtId, DateTime date);
    }

    public class CourtService : ICourtService
    {
        private readonly ApplicationDbContext _context;

        public CourtService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Court>> GetAllCourtsAsync()
        {
            return await _context.Courts.Where(c => c.IsActive).ToListAsync();
        }

        public async Task<Court?> GetCourtByIdAsync(int courtId)
        {
            return await _context.Courts.FindAsync(courtId);
        }

        public async Task<Court> CreateCourtAsync(string name, string? description = null)
        {
            var court = new Court
            {
                Name = name,
                Description = description,
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            _context.Courts.Add(court);
            await _context.SaveChangesAsync();
            return court;
        }

        public async Task UpdateCourtAsync(int courtId, string name, bool isActive, string? description)
        {
            var court = await _context.Courts.FindAsync(courtId);
            if (court != null)
            {
                court.Name = name;
                court.IsActive = isActive;
                court.Description = description;
                court.ModifiedDate = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteCourtAsync(int courtId)
        {
            var court = await _context.Courts.FindAsync(courtId);
            if (court != null)
            {
                _context.Courts.Remove(court);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Kiểm tra xem khung giờ có sẵn trên sân hay không
        /// </summary>
        public async Task<bool> IsTimeSlotAvailableAsync(int courtId, DateTime startTime, DateTime endTime)
        {
            // Kiểm tra xem có booking nào trùng lịch không
            var conflictingBooking = await _context.Bookings
                .Where(b => b.CourtId == courtId
                    && b.Status != BookingStatus.Cancelled
                    && b.StartTime < endTime
                    && b.EndTime > startTime)
                .FirstOrDefaultAsync();

            return conflictingBooking == null;
        }

        public async Task<List<Booking>> GetCourtBookingsAsync(int courtId, DateTime date)
        {
            var startOfDay = date.Date;
            var endOfDay = startOfDay.AddDays(1);

            return await _context.Bookings
                .Include(b => b.Member)
                .Where(b => b.CourtId == courtId
                    && b.StartTime >= startOfDay
                    && b.StartTime < endOfDay
                    && b.Status != BookingStatus.Cancelled)
                .OrderBy(b => b.StartTime)
                .ToListAsync();
        }
    }
}
