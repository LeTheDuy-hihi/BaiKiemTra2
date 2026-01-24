using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PickleballClubManagement.Models
{
    [Table("186_Members")]
    public class Member
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string IdentityUserId { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Active"; // "Active" or "Block"

        [Required]
        public double RankLevel { get; set; } = 1.0;
        
        public DateTime? DateOfBirth { get; set; }
        public int TotalMatches { get; set; }
        public int WinMatches { get; set; }
        
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }

        // Navigation property
        [ForeignKey("IdentityUserId")]
        public virtual IdentityUser? User { get; set; }

        public virtual ICollection<Challenge> CreatedChallenges { get; set; } = new List<Challenge>();
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
