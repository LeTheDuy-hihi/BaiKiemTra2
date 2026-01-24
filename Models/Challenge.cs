using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PickleballClubManagement.Models
{
    public enum ChallengeStatus
    {
        Open = 0,
        Accepted = 1,     // For Duels
        Ongoing = 2,      // For MiniGames
        Completed = 3,    // For Duels
        Finished = 4      // For MiniGames
    }

    [Table("186_Challenges")]
    public class Challenge
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CreatorId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public ChallengeStatus Status { get; set; } = ChallengeStatus.Open;

        // Advanced Features
        public ChallengeType Type { get; set; } = ChallengeType.Duel;
        public GameMode ResultMode { get; set; } = GameMode.None;
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal EntryFee { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrizePool { get; set; }
        
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        
        // For Team Battle
        public int? Config_TargetWins { get; set; }
        public int CurrentScore_TeamA { get; set; }
        public int CurrentScore_TeamB { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }

        // Navigation properties
        [ForeignKey("CreatorId")]
        public virtual Member? Creator { get; set; }

        public virtual ICollection<Match> Matches { get; set; } = new List<Match>();
        public virtual ICollection<Participant> Participants { get; set; } = new List<Participant>();
    }

    public enum ChallengeType
    {
        Duel = 0,
        MiniGame = 1
    }

    public enum GameMode
    {
        None = 0,
        TeamBattle = 1,
        RoundRobin = 2
    }
}
