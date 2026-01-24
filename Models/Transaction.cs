using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PickleballClubManagement.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        
        [Required]
        public DateTime Date { get; set; } = DateTime.Now;
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; } // >0 for Income, <0 for Expense usually handled by Type
        
        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public TransactionCategory? Category { get; set; }
        
        public string? CreatedById { get; set; } // Identity User Id
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
