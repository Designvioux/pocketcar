using CarRentalApp.Data;
using CarRentalApp.Models.Entities;
using CarRentalApp.Models.EntityDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly WheelzOnDbContext dbContext;

        public CarsController(WheelzOnDbContext DbContext)
        {
            dbContext = DbContext;
        }

        /*[HttpGet]
        public IActionResult GetAllCars()
        {
            var allCars = dbContext.Cars.ToList();
            return Ok(allCars);
        }*/

        /*[HttpGet]
        [Route("{id:int}")]
        public IActionResult GetCarById(int id) 
        {
            var car = dbContext.Cars
        .Where(c => c.Id == id)
        .Select(c => new
        {
            Name = c.Name,
            CarNumber = c.CarNumber,
            SeatingCapacity = c.SeatingCapacity,
            FuelType = c.FuelType,
            ACAvailable = c.ACAvailable,
            CarPhotos = c.CarPhotos,

            Driver = new
            {
                Name = c.DriverName,
                PhoneNumber = c.DriverPhoneNumber,
                Email = c.DriverEmail,
                BloodGroup = c.DriverBloodGroup,
                Location = c.DriverLocation
            }
        })
        .FirstOrDefault();

            if (car == null)
            {
                return NotFound();
            }

            return Ok(car);
        }*/


        //Working newest
        /*[HttpGet]
        [Route("{id:int}")]
        public IActionResult GetCarById(int id)
        {
            var carEntity = dbContext.Cars.FirstOrDefault(c => c.Id == id);

            if (carEntity == null)
            {
                return NotFound();
            }

            var carDto = new
            {
                Name = carEntity.Name,
                CarNumber = carEntity.CarNumber,
                SeatingCapacity = carEntity.SeatingCapacity,
                FuelType = carEntity.FuelType,
                ACAvailable = carEntity.ACAvailable,

                CarPhotos = string.IsNullOrEmpty(carEntity.CarPhotos)
                                ? new List<string>()
                                : carEntity.CarPhotos.Split(',').ToList(),

                CarDocuments = string.IsNullOrEmpty(carEntity.CarDocuments)
                                ? new List<string>()
                                : carEntity.CarDocuments.Split(',').ToList(),

                Driver = new
                {
                    Name = carEntity.DriverName,
                    PhoneNumber = carEntity.DriverPhoneNumber,
                    Email = carEntity.DriverEmail,
                    BloodGroup = carEntity.DriverBloodGroup,
                    Location = carEntity.DriverLocation
                }
            };

            return Ok(carDto);
        }*/

        /*[HttpPost("upload")]
        public async Task<IActionResult> UploadCarDocuments(int carId, List<IFormFile> files)
        {
            var car = dbContext.Cars.Find(carId);
            if (car == null) return NotFound();

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/");
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            List<string> filePaths = new();
            foreach (var file in files)
            {
                var filePath = Path.Combine("uploads", file.FileName);
                using (var stream = new FileStream(Path.Combine(uploadsFolder, file.FileName), FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                filePaths.Add(filePath);
            }

            car.CarDocuments = string.Join(",", filePaths);
            dbContext.SaveChanges();

            return Ok(new { message = "Files uploaded successfully", files = filePaths });
        }*/


        /*[HttpGet("{id}")]
        public IActionResult GetCarById(int id)
        {
            var car = dbContext.Cars.Find(id);
            if (car == null) return NotFound();

            var baseUrl = $"{Request.Scheme}://{Request.Host}/";

            var carDto = new CarsDto
            {
                Name = car.Name,
                CarNumber = car.CarNumber,
                SeatingCapacity = car.SeatingCapacity,
                FuelType = car.FuelType,
                ACAvailable = car.ACAvailable,
                CarPhotoUrls = car.CarPhotos?.Split(',').Select(photo => baseUrl + photo).ToList(),
                CarDocumentUrls = car.CarDocuments?.Split(',').Select(doc => baseUrl + doc).ToList(),
                Driver = new DriverDto
                {
                    Name = car.DriverName,
                    PhoneNumber = car.DriverPhoneNumber,
                    Email = car.DriverEmail,
                    BloodGroup = car.DriverBloodGroup,
                    Location = car.DriverLocation
                }
            };

            return Ok(carDto);
        }

        // Function to map document index to a predefined name
        private string GetDocumentName(int index)
        {
            string[] documentNames = { "Fitness", "Insurance", "Tax", "PUC", "Permit" };
            return index < documentNames.Length ? documentNames[index] : $"Document {index + 1}";
        }*/



        /*[HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddCars([FromForm] CarsDto carsDto)
        {
            try
            {
                // Validate if the request contains data
                if (carsDto == null)
                    return BadRequest("Invalid request. No data received.");
                var carEntity = new Car()

            {
                Name = carsDto.Name,
                CarNumber = carsDto.CarNumber,
                SeatingCapacity = carsDto.SeatingCapacity,
                FuelType = carsDto.FuelType,
                ACAvailable = carsDto.ACAvailable,
                DriverName = carsDto.Driver.Name,
                DriverPhoneNumber = carsDto.Driver.PhoneNumber,
                DriverEmail = carsDto.Driver.Email,
                DriverBloodGroup = carsDto.Driver.BloodGroup,
                DriverLocation = carsDto.Driver.Location,
                CarPhotos = "",
            };

            var imageUrls = new List<string>();
            foreach (var file in carsDto.CarPhotos)
            {
                if (file != null && file.Length > 0)
                {
                    var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                    var filePath = Path.Combine("wwwroot/CarImages", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Store only relative path
                    imageUrls.Add($"/CarImages/{fileName}");
                }
            }

            carEntity.CarPhotos = string.Join(",", imageUrls); // Store as CSV

            dbContext.Cars.Add(carEntity);
            await dbContext.SaveChangesAsync();

            return Ok(carEntity);
        }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddCars: {ex.Message}");
                return StatusCode(500, new { error = "An error occurred while processing your request.", details = ex.Message });
            }
        }*/

        /*[HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddCars([FromForm] CarsDto carsDto, [FromForm] List<IFormFile> carDocuments)
        {
            try
            {
                if (carsDto == null)
                    return BadRequest("Invalid request. No data received.");

                var carEntity = new Car()
                {
                    Name = carsDto.Name,
                    CarNumber = carsDto.CarNumber,
                    SeatingCapacity = carsDto.SeatingCapacity,
                    FuelType = carsDto.FuelType,
                    ACAvailable = carsDto.ACAvailable,
                    DriverName = carsDto.Driver.Name,
                    DriverPhoneNumber = carsDto.Driver.PhoneNumber,
                    DriverEmail = carsDto.Driver.Email,
                    DriverBloodGroup = carsDto.Driver.BloodGroup,
                    DriverLocation = carsDto.Driver.Location,
                    CarPhotos = "",
                    CarDocuments = ""
                };

                var carImagesFolder = Path.Combine("wwwroot", "CarImages");
                var carDocumentsFolder = Path.Combine("wwwroot", "CarDocuments");

                if (!Directory.Exists(carImagesFolder))
                {
                    Directory.CreateDirectory(carImagesFolder);
                }

                if (!Directory.Exists(carDocumentsFolder))
                {
                    Directory.CreateDirectory(carDocumentsFolder);
                }

                var imageUrls = new List<string>();
                foreach (var file in carsDto.CarPhotos)
                {
                    if (file != null && file.Length > 0)
                    {
                        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                        var filePath = Path.Combine("wwwroot/CarImages", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        imageUrls.Add($"/CarImages/{fileName}"); 
                    }
                }
                carEntity.CarPhotos = string.Join(",", imageUrls); 

                var documentUrls = new List<string>();
                foreach (var file in carDocuments)
                {
                    if (file != null && file.Length > 0 && Path.GetExtension(file.FileName).ToLower() == ".pdf")
                    {
                        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                        var filePath = Path.Combine("wwwroot/CarDocuments", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        documentUrls.Add($"/CarDocuments/{fileName}"); 
                    }
                }
                carEntity.CarDocuments = string.Join(",", documentUrls); 

                dbContext.Cars.Add(carEntity);
                await dbContext.SaveChangesAsync();

                return Ok(carEntity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddCars: {ex.Message}");
                return StatusCode(500, new { error = "An error occurred while processing your request.", details = ex.Message });
            }
        }*/


        /*[HttpPut]
        [Route("{id:int}")]

        public async Task<IActionResult> UpdateCars(int id, [FromForm] CarsDto carsDto)
        {
            var car = dbContext.Cars.Find(id);
            if(car == null)
            {
                return NotFound();
            }

            car.Name = carsDto.Name;
            car.CarNumber = carsDto.CarNumber;
            car.FuelType = carsDto.FuelType;
            car.ACAvailable = carsDto.ACAvailable;
            car.SeatingCapacity = carsDto.SeatingCapacity;
            car.DriverName = carsDto.Driver.Name;
            car.DriverPhoneNumber = carsDto.Driver.PhoneNumber;
            car.DriverEmail = carsDto.Driver.Email;
            car.DriverBloodGroup = carsDto.Driver.BloodGroup;
            car.DriverLocation = carsDto.Driver.Location;

            var imageUrls = new List<string>();
            foreach (var file in carsDto.CarPhotos)
            {
                if (file != null && file.Length > 0)
                {
                    var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                    var filePath = Path.Combine("wwwroot/CarImages", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    imageUrls.Add($"/CarImages/{fileName}");
                }
            }

            if (imageUrls.Count > 0)
            {
                car.CarPhotos = string.Join(",", imageUrls); // Update images if provided
            }

            await dbContext.SaveChangesAsync();
            return Ok(car);
        }*/

        /*[HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateCars(int id, [FromForm] CarsDto carsDto)
        {
            var car = dbContext.Cars.Find(id);
            if (car == null)
            {
                return NotFound();
            }

            car.Name = carsDto.Name;
            car.CarNumber = carsDto.CarNumber;
            car.FuelType = carsDto.FuelType;
            car.ACAvailable = carsDto.ACAvailable;
            car.SeatingCapacity = carsDto.SeatingCapacity;
            car.DriverName = carsDto.Driver.Name;
            car.DriverPhoneNumber = carsDto.Driver.PhoneNumber;
            car.DriverEmail = carsDto.Driver.Email;
            car.DriverBloodGroup = carsDto.Driver.BloodGroup;
            car.DriverLocation = carsDto.Driver.Location;

            var imageUrls = new List<string>();
            if (carsDto.CarPhotos != null && carsDto.CarPhotos.Count > 0)
            {
                foreach (var file in carsDto.CarPhotos)
                {
                    if (file != null && file.Length > 0)
                    {
                        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                        var filePath = Path.Combine("wwwroot/CarImages", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        imageUrls.Add($"/CarImages/{fileName}");
                    }
                }
                car.CarPhotos = string.Join(",", imageUrls);
            }
            else if (!string.IsNullOrEmpty(car.CarPhotos))
            {
                imageUrls.AddRange(car.CarPhotos.Split(',')); 
            }

            var docUrls = new List<string>();
            if (carsDto.CarDocuments != null && carsDto.CarDocuments.Count > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                foreach (var file in carsDto.CarDocuments)
                {
                    if (file != null && file.Length > 0)
                    {
                        var filePath = Path.Combine("uploads", file.FileName);
                        using (var stream = new FileStream(Path.Combine(uploadsFolder, file.FileName), FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        docUrls.Add(filePath);
                    }
                }
                car.CarDocuments = string.Join(",", docUrls); 
            }
            else if (!string.IsNullOrEmpty(car.CarDocuments))
            {
                docUrls.AddRange(car.CarDocuments.Split(',')); 
            }

            await dbContext.SaveChangesAsync();
            return Ok(car);
        }*/

        /*[HttpDelete]
        [Route("{id:int}")]
        public IActionResult DeleteCar(int id)
        {
            var car = dbContext.Cars.Find(id);
            if(car == null)
            {
                return NotFound();
            }

            dbContext.Cars.Remove(car);
            dbContext.SaveChanges();

            return Ok(car);

        }*/


        private async Task<string> SaveFile(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
                return null;

            var folderPath = Path.Combine("wwwroot", folderName);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/{folderName}/{fileName}";
        }

        // ✅ **1️⃣ Add a Car with Documents**
        [HttpPost("add")]
        public async Task<IActionResult> AddCar([FromForm] CarsDto carsDto)
        {
            try
            {
                if (carsDto == null)
                    return BadRequest("Invalid request. No data received.");

                var carEntity = new Car
                {
                    Name = carsDto.Name,
                    CarNumber = carsDto.CarNumber,
                    SeatingCapacity = carsDto.SeatingCapacity,
                    FuelType = carsDto.FuelType,
                    ACAvailable = carsDto.ACAvailable,
                    DriverName = carsDto.Driver?.Name,
                    DriverPhoneNumber = carsDto.Driver?.PhoneNumber,
                    DriverEmail = carsDto.Driver?.Email,
                    DriverBloodGroup = carsDto.Driver?.BloodGroup,
                    DriverLocation = carsDto.Driver?.Location
                };

                // Saving Photos
                if (carsDto.CarPhotos != null)
                {
                    var imageUrls = new List<string>();
                    foreach (var file in carsDto.CarPhotos)
                    {
                        var url = await SaveFile(file, "CarImages");
                        if (url != null) imageUrls.Add(url);
                    }
                    carEntity.CarPhotos = string.Join(",", imageUrls);
                }

                // Saving Specific Documents
                carEntity.FitnessCertificate = await SaveFile(carsDto.FitnessCertificate, "CarDocuments");
                carEntity.Insurance = await SaveFile(carsDto.Insurance, "CarDocuments");
                carEntity.Tax = await SaveFile(carsDto.Tax, "CarDocuments");
                carEntity.PUC = await SaveFile(carsDto.PUC, "CarDocuments");
                carEntity.Permit = await SaveFile(carsDto.Permit, "CarDocuments");

                dbContext.Cars.Add(carEntity);
                await dbContext.SaveChangesAsync();

                return Ok(carEntity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        // ✅ **2️⃣ Get All Cars**
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllCars()
        {
            var cars = await dbContext.Cars.ToListAsync();
            return Ok(cars);
        }

        // ✅ **3️⃣ Get a Car by ID**
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetCar(int id)
        {
            var car = await dbContext.Cars.FindAsync(id);
            if (car == null)
                return NotFound("Car not found.");

            return Ok(car);
        }

        // ✅ **4️⃣ Update a Car and Specific Documents**
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateCar(int id, [FromForm] CarsDto carsDto)
        {
            var car = await dbContext.Cars.FindAsync(id);
            if (car == null)
                return NotFound("Car not found.");

            car.Name = carsDto.Name;
            car.CarNumber = carsDto.CarNumber;
            car.SeatingCapacity = carsDto.SeatingCapacity;
            car.FuelType = carsDto.FuelType;
            car.ACAvailable = carsDto.ACAvailable;
            car.DriverName = carsDto.Driver?.Name;
            car.DriverPhoneNumber = carsDto.Driver?.PhoneNumber;
            car.DriverEmail = carsDto.Driver?.Email;
            car.DriverBloodGroup = carsDto.Driver?.BloodGroup;
            car.DriverLocation = carsDto.Driver?.Location;

            // Updating Photos
            if (carsDto.CarPhotos != null && carsDto.CarPhotos.Any())
            {
                var imageUrls = new List<string>();
                foreach (var file in carsDto.CarPhotos)
                {
                    var url = await SaveFile(file, "CarImages");
                    if (url != null) imageUrls.Add(url);
                }
                car.CarPhotos = string.Join(",", imageUrls);
            }

            // Updating Specific Documents
            car.FitnessCertificate = carsDto.FitnessCertificate != null ? await SaveFile(carsDto.FitnessCertificate, "CarDocuments") : car.FitnessCertificate;
            car.Insurance = carsDto.Insurance != null ? await SaveFile(carsDto.Insurance, "CarDocuments") : car.Insurance;
            car.Tax = carsDto.Tax != null ? await SaveFile(carsDto.Tax, "CarDocuments") : car.Tax;
            car.PUC = carsDto.PUC != null ? await SaveFile(carsDto.PUC, "CarDocuments") : car.PUC;
            car.Permit = carsDto.Permit != null ? await SaveFile(carsDto.Permit, "CarDocuments") : car.Permit;

            await dbContext.SaveChangesAsync();
            return Ok(car);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var car = await dbContext.Cars.FindAsync(id);
            if (car == null)
                return NotFound("Car not found.");

            dbContext.Cars.Remove(car);
            await dbContext.SaveChangesAsync();
            return Ok("Car deleted successfully.");
        }

        // ✅ **6️⃣ Get Specific Document for a Car**
        [HttpGet("getDocument/{id}/{documentType}")]
        public async Task<IActionResult> GetCarDocument(int id, string documentType)
        {
            var car = await dbContext.Cars.FindAsync(id);
            if (car == null)
                return NotFound("Car not found.");

            string documentUrl = documentType.ToLower() switch
            {
                "fitness" => car.FitnessCertificate,
                "insurance" => car.Insurance,
                "tax" => car.Tax,
                "puc" => car.PUC,
                "permit" => car.Permit,
                _ => null
            };

            if (string.IsNullOrEmpty(documentUrl))
                return NotFound($"Document '{documentType}' not found.");

            return Ok(new { DocumentType = documentType, DocumentUrl = documentUrl });
        }





}
}
