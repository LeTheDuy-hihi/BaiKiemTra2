using System.ComponentModel.DataAnnotations;

namespace PickleballClubManagement.Models
{
    public class TransactionCategory
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        public TransactionType Type { get; set; }
    }

    public enum TransactionType
    {
        Income = 0, // Thu
        Expense = 1 // Chi
    }
}
