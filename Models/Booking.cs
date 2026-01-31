using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PickleballClubManagement.Models
{
    [Table("186_Bookings")]
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MemberId { get; set; }

        public int? CourtId { get; set; }
        [ForeignKey("CourtId")]
        public virtual Court? Court { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        public BookingStatus Status { get; set; } = BookingStatus.Confirmed;

        [StringLength(500)]
        public string? Notes { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }

        // Navigation property
        [ForeignKey("MemberId")]
        public virtual Member? Member { get; set; }
    }

    public enum BookingStatus
    {
        Pending = 0,
        Confirmed = 1,
        Cancelled = 2
    }
}
