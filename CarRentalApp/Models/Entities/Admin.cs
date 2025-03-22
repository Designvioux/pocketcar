using System.ComponentModel.DataAnnotations;

namespace CarRentalApp.Models.Entities
{
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
