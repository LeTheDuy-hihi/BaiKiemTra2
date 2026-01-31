using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PickleballClubManagement.Models
{
    public enum TeamName
    {
        None = 0,
        TeamA = 1,
        TeamB = 2
    }

    public enum ParticipantStatus
    {
        Pending = 0,
        Confirmed = 1,
        Withdrawn = 2
    }

    [Table("186_Participants")]
    public class Participant
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int ChallengeId { get; set; }

        [ForeignKey("ChallengeId")]
        public virtual Challenge? Challenge { get; set; }
        
        [Required]
        public int MemberId { get; set; }

        [ForeignKey("MemberId")]
        public virtual Member? Member { get; set; }
        
        [Required]
        public TeamName Team { get; set; } = TeamName.None;
        
        [Required]
        public bool EntryFeePaid { get; set; } = false;
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal EntryFeeAmount { get; set; }
        
        [Required]
        public ParticipantStatus Status { get; set; } = ParticipantStatus.Pending;
        
        public DateTime JoinedDate { get; set; } = DateTime.Now;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
