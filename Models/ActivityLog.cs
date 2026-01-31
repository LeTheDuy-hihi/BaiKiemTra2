using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PickleballClubManagement.Models
{
    [Table("186_ActivityLogs")]
    public class ActivityLog
    {
        [Key]
        public int Id { get; set; }

        public string? UserId { get; set; } // Identity User Id

        [Required]
        [StringLength(100)]
        public string Action { get; set; } = string.Empty; // "Create", "Update", "Delete", etc.

        [Required]
        [StringLength(50)]
        public string EntityType { get; set; } = string.Empty; // "Match", "Member", "Challenge", etc.

        public int? EntityId { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [StringLength(50)]
        public string? IpAddress { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
