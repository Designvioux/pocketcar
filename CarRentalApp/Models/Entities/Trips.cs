using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalApp.Models.Entities
{
    [Table("trips")]
    public class Trips
    {
        [Key]
        public int TripID { get; set; }

        // Car Details
        [Required]
        [MaxLength(255)]
        public string CarName { get; set; }

        [Required]
        [MaxLength(50)]
        public string CarNumber { get; set; }

        [Required]
        public int KilometerFrom { get; set; }

        [Required]
        public int KilometerTo { get; set; }

        // Driver Details
        [Required]
        [MaxLength(255)]
        public string DriverName { get; set; }

        [Required]
        [MaxLength(255)]
        public string DriverNumber { get; set; }

        public string DriverBloodGroup { get; set; }

        [Required]
        [MaxLength(255)]
        public string DriverLicense { get; set; }

        [Required]
        [MaxLength(12)]
        public string DriverAadharNumber { get; set; }

        // Customer Details
        [Required]
        [MaxLength(255)]
        public string CustomerName { get; set; }

        [Required]
        [MaxLength(15)]
        public string CustomerContact { get; set; }

        [Required]
        [MaxLength(255)]
        public string PickUpLocation { get; set; }

        [Required]
        public DateTime PickUpDate { get; set; }

        [Required]
        public TimeSpan PickUpTime { get; set; }

        [Required]
        [MaxLength(255)]
        public string DropOffLocation { get; set; }

        [Required]
        public DateTime DropOffDate { get; set; }

        [Required]
        public TimeSpan DropOffTime { get; set; }

        [Required]
        public decimal Price { get; set; }


        // Additional Information
        public string Remark { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
