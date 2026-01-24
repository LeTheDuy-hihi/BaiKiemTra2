using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PickleballClubManagement.Models
{
    public enum MatchFormat
    {
        Singles = 0,  // Đơn (1v1)
        Doubles = 1   // Đôi (2v2)
    }

    [Table("186_Matches")]
    public class Match
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime MatchDate { get; set; } = DateTime.Now;

        [Required]
        public MatchFormat Format { get; set; }

        [Required]
        public bool IsRanked { get; set; } = true;

        // Nullable - null if free play (không có kèo)
        public int? ChallengeId { get; set; }

        // Winners
        [Required]
        public int Winner1Id { get; set; }

        // Nullable - only for Doubles
        public int? Winner2Id { get; set; }

        // Losers
        [Required]
        public int Loser1Id { get; set; }

        // Nullable - only for Doubles
        public int? Loser2Id { get; set; }

        // Navigation properties
        [ForeignKey("ChallengeId")]
        public virtual Challenge? Challenge { get; set; }

        [ForeignKey("Winner1Id")]
        public virtual Member? Winner1 { get; set; }

        [ForeignKey("Winner2Id")]
        public virtual Member? Winner2 { get; set; }

        [ForeignKey("Loser1Id")]
        public virtual Member? Loser1 { get; set; }

        [ForeignKey("Loser2Id")]
        public virtual Member? Loser2 { get; set; }

        public WinningSide WinningSide { get; set; } = WinningSide.None;
    }

    public enum WinningSide
    {
        None = 0,
        Team1 = 1,
        Team2 = 2
    }
}
