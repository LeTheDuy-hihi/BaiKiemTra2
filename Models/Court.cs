using System.ComponentModel.DataAnnotations;

namespace PickleballClubManagement.Models
{
    public class Court
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        public bool IsActive { get; set; } = true;
        
        [StringLength(500)]
        public string? Description { get; set; }
    }
}
