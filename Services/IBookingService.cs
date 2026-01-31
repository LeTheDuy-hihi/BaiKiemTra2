using PickleballClubManagement.Models;

namespace PickleballClubManagement.Services
{
    public interface IBookingService
    {
        Task<bool> IsTimeSlotAvailableAsync(int courtId, DateTime startTime, DateTime endTime);
        Task<Booking> CreateBookingAsync(int memberId, int courtId, DateTime startTime, DateTime endTime, string? notes);
        Task<List<Booking>> GetBookingsByDateAsync(DateTime date);
        Task<List<Booking>> GetMemberBookingsAsync(int memberId);
        Task<Booking?> GetBookingByIdAsync(int bookingId);
        Task UpdateBookingAsync(int bookingId, BookingStatus status, string? notes);
        Task CancelBookingAsync(int bookingId);
    }
}
