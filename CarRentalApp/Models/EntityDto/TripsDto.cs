namespace CarRentalApp.Models.EntityDto;
using System.ComponentModel.DataAnnotations;


public class TripsDto
{
    public int TripID { get; set; }

    // Car Details
    [Required] public string CarName { get; set; }
    [Required] public string CarNumber { get; set; }
    [Required] public int KilometerFrom { get; set; }
    [Required] public int KilometerTo { get; set; }

    // Driver Details
    [Required] public string DriverName { get; set; }
    [Required] public string DriverNumber { get; set; }
    public string DriverBloodGroup { get; set; }
    [Required] public string DriverLicense { get; set; }
    [Required] public string DriverAadharNumber { get; set; }

    // Customer Details
    [Required] public string CustomerName { get; set; }
    [Required] public string CustomerContact { get; set; }

    // Trip Details
    [Required] public string PickUpLocation { get; set; }
    [Required] public DateTime PickUpDate { get; set; }
    [Required] public TimeSpan PickUpTime { get; set; }
    [Required] public string DropOffLocation { get; set; }
    [Required] public DateTime DropOffDate { get; set; }
    [Required] public TimeSpan DropOffTime { get; set; }

    [Required] public decimal Price { get; set; }
    public string Remark { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
