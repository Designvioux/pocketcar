using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalApp.Models.Entities
{
    [Table("cars")]
    public class Car
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string CarNumber { get; set; }
        public int SeatingCapacity { get; set; }
        public string? FuelType { get; set; }
        public bool ACAvailable { get; set; }
        public string? CarPhotos { get; set; }
        public string? FitnessCertificate { get; set; }
        public string? Insurance { get; set; }
        public string? Tax { get; set; }
        public string? PUC { get; set; }
        public string? Permit { get; set; }

        public string DriverName { get; set; }
        public string DriverPhoneNumber { get; set; }
        public string DriverEmail { get; set; }
        public string? DriverBloodGroup { get; set; }
        public string? DriverLocation { get; set; }
    }
}
