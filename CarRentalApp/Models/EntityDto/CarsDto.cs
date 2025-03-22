using System.ComponentModel.DataAnnotations;

namespace CarRentalApp.Models.EntityDto
{
    public class CarsDto
    {
        public string Name { get; set; }
        public string CarNumber { get; set; }
        public int SeatingCapacity { get; set; } 
        public string? FuelType { get; set; } 
        public bool ACAvailable { get; set; }
        public List<IFormFile>? CarPhotos { get; set; } = new List<IFormFile>();
        public List<string>? CarPhotoUrls { get; set; }
        public IFormFile? FitnessCertificate { get; set; }
        public IFormFile? Insurance { get; set; }
        public IFormFile? Tax { get; set; }
        public IFormFile? PUC { get; set; }
        public IFormFile? Permit { get; set; }

        public string? FitnessCertificateUrl { get; set; }
        public string? InsuranceUrl { get; set; }
        public string? TaxUrl { get; set; }
        public string? PUCUrl { get; set; }
        public string? PermitUrl { get; set; }
        public DriverDto? Driver { get; set; } 
    }


    public class DriverDto
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string BloodGroup { get; set; }
        public string Location { get; set; }
    }
}
