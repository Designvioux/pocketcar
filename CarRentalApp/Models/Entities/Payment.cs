using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalApp.Models.Entities
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        [Required]
        public int BookingId { get; set; }
        [ForeignKey("BookingId")]
        public Booking Booking {get; set;}
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string PaymentMethod { get; set; } 

        public string Status { get; set; } = "Pending"; 

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

    }
}
