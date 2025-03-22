using System.ComponentModel.DataAnnotations;

namespace CarRentalApp.Models.EntityDto
{
    public class UserDto
    {
        public string FullName { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string Phone { get; set; }

        public string Role { get; set; } = "Customer";
    }
}
