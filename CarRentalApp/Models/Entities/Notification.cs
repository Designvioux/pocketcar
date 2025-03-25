using System.ComponentModel.DataAnnotations;

namespace CarRentalApp.Models.Entities
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;
    }
}
