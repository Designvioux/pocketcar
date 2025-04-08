using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalApp.Models.Entities
{
    [Table("admin")]
    public class Admin
    {
        [Key]
        public int Id { get; set; } 
        [Required]
        public string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; }
    }
}
