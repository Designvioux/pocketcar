using System.ComponentModel.DataAnnotations;

namespace CarRentalApp.Models.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string Phone { get; set; }

        public string Role { get; set; } = "Customer";
    }
}
