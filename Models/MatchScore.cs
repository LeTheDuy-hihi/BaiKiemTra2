using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PickleballClubManagement.Models
{
    [Table("186_MatchScores")]
    public class MatchScore
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MatchId { get; set; }

        [Required]
        public int SetNumber { get; set; }

        [Required]
        public int Team1Score { get; set; }

        [Required]
        public int Team2Score { get; set; }

        public bool IsFinalSet { get; set; } = false;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [ForeignKey("MatchId")]
        public virtual Match? Match { get; set; }
    }
}
