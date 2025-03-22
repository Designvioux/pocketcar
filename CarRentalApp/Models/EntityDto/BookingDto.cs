using CarRentalApp.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CarRentalApp.Models.EntityDto
{
    public class BookingDto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [StringLength(10)]
        public string ContactNumber { get; set; }

        [Required]
        public string SelectedCar { get; set; }

        [Required]
        public string PickupLocation { get; set; }

        [Required]
        public DateTime PickupDate { get; set; }

        [Required]
        public DateTime PickupTime { get; set; }

        [Required]
        public string DropLocation { get; set; }

        [Required]
        public DateTime DropDate { get; set; }

        [Required]
        public DateTime DropOffTime { get; set; }

        [Required]
        public string TripType { get; set; }
        public string? Status { get; set; } = "Pending";
    }
}
