using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PickleballClubManagement.Models
{
    public class Participant
    {
        public int Id { get; set; }
        
        public int ChallengeId { get; set; }
        [ForeignKey("ChallengeId")]
        public Challenge? Challenge { get; set; }
        
        public int MemberId { get; set; }
        [ForeignKey("MemberId")]
        public Member? Member { get; set; }
        
        public TeamName Team { get; set; } = TeamName.None;
        
        public bool EntryFeePaid { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal EntryFeeAmount { get; set; }
        
        public ParticipantStatus Status { get; set; } = ParticipantStatus.Pending;
        
        public DateTime JoinedDate { get; set; } = DateTime.Now;
    }

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
}
