using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PickleballClubManagement.Models
{
    public enum NotificationType
    {
        Info = 0,
        Warning = 1,
        Success = 2,
        Error = 3
    }

    [Table("186_Notifications")]
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Content { get; set; } = string.Empty;

        [Required]
        public NotificationType Type { get; set; }

        public bool IsRead { get; set; } = false;

        // Null = notification for all members
        public int? MemberId { get; set; }

        [StringLength(500)]
        public string? LinkUrl { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [ForeignKey("MemberId")]
        public virtual Member? Member { get; set; }
    }
}
