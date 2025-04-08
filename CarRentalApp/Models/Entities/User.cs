using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalApp.Models.Entities
{
    [Table("users")]
    public class User
    {
        [Key]
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public DateTime DateOfBirth { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Gender { get; set; }

        public string City { get; set; }

        public string? ProfilePictureUrl { get; set; } = string.Empty;

        public bool IsDeleted { get; set; } = false;
    }
}
