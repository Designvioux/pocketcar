using CarRentalApp.Data;
using CarRentalApp.Models.Entities;
using CarRentalApp.Models.EntityDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Helpers;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CarRentalApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripController : Controller
    {
        private readonly WheelzOnDbContext dbContext;

        public TripController(WheelzOnDbContext DbContext)
        {
            dbContext = DbContext;
        }

        //Old Method
        /*[HttpGet]
        public async Task<IActionResult> GetTrips()
        {
            var trips = await dbContext.Trips.ToListAsync();

            var tripsDto = trips.Select(trip => new TripsDto
            {
                TripID = trip.TripID,
                PickUpLocation = trip.PickUpLocation,
                //PickUpDateTime = trip.PickUpDateTime,
                PickUpDate = trip.PickUpDate,
                PickUpTime = trip.PickUpTime,
                DropOffLocation = trip.DropOffLocation,
                DropOffDate = trip.DropOffDate,
                DropOffTime = trip.DropOffTime,
                //DropOffDateTime = trip.DropOffDateTime,
                KilometerFrom = trip.KilometerFrom,
                KilometerTo = trip.KilometerTo,
                Price = trip.Price,
                CarName = trip.CarName,
                CarNumber = trip.CarNumber,
                DriverName = trip.DriverName,
                DriverNumber = trip.DriverNumber,
                DriverBloodGroup = trip.DriverBloodGroup,
                CustomerName = trip.CustomerName,
                CustomerContact = trip.CustomerContact,
                Remark = trip.Remark,
                CreatedAt = trip.CreatedAt
            }).ToList();

            return Ok(tripsDto);
        }*/

        [HttpGet]
        public async Task<IActionResult> GetTrips(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string searchCarNumber = "",
    [FromQuery] string searchDriverName = "",
    [FromQuery] string searchDriverLicense = "",
    [FromQuery] string searchDriverAadharNumber = "",
    [FromQuery] string searchPickUpLocation = "",
    [FromQuery] string searchDropOffLocation = "",
    [FromQuery] string searchCustomerName = "",
    [FromQuery] string searchCustomerContact = "",
    [FromQuery] int? searchKilometerFrom = null,
    [FromQuery] int? searchKilometerTo = null,
    [FromQuery] decimal? searchPrice = null,
    [FromQuery] DateTime? pickUpDate = null,
    [FromQuery] DateTime? dropOffDate = null
)
        {
            var query = dbContext.Trips.AsQueryable();

            // Apply Filters
            if (!string.IsNullOrEmpty(searchCarNumber))
                query = query.Where(t => t.CarNumber.Contains(searchCarNumber));

            if (!string.IsNullOrEmpty(searchDriverName))
                query = query.Where(t => t.DriverName.Contains(searchDriverName));

            if (!string.IsNullOrEmpty(searchDriverLicense))
                query = query.Where(t => t.DriverAadharNumber.Contains(searchDriverLicense));

            if (!string.IsNullOrEmpty(searchDriverAadharNumber))
                query = query.Where(t => t.DriverLicense.Contains(searchDriverAadharNumber));

            if (!string.IsNullOrEmpty(searchPickUpLocation))
                query = query.Where(t => t.PickUpLocation.Contains(searchPickUpLocation));

            if (!string.IsNullOrEmpty(searchDropOffLocation))
                query = query.Where(t => t.DropOffLocation.Contains(searchDropOffLocation));

            if (!string.IsNullOrEmpty(searchCustomerName))
                query = query.Where(t => t.CustomerName.Contains(searchCustomerName));

            if (!string.IsNullOrEmpty(searchCustomerContact))
                query = query.Where(t => t.CustomerContact.Contains(searchCustomerContact));

            if (searchKilometerFrom.HasValue)
                query = query.Where(t => t.KilometerFrom >= searchKilometerFrom.Value);

            if (searchKilometerTo.HasValue)
                query = query.Where(t => t.KilometerTo <= searchKilometerTo.Value);

            if (searchPrice.HasValue)
                query = query.Where(t => t.Price == searchPrice.Value);

            // Date Filters
            if (pickUpDate.HasValue)
                query = query.Where(t => t.PickUpDate.Date == pickUpDate.Value.Date);

            if (dropOffDate.HasValue)
                query = query.Where(t => t.DropOffDate.Date == dropOffDate.Value.Date);

            // Pagination
            var totalRecords = await query.CountAsync();
            var trips = await query
                .OrderByDescending(t => t.CreatedAt) // Order by latest trips
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var tripsDto = trips.Select(trip => new TripsDto
            {
                TripID = trip.TripID,
                PickUpLocation = trip.PickUpLocation,
                PickUpDate = trip.PickUpDate,
                PickUpTime = trip.PickUpTime,
                DropOffLocation = trip.DropOffLocation,
                DropOffDate = trip.DropOffDate,
                DropOffTime = trip.DropOffTime,
                KilometerFrom = trip.KilometerFrom,
                KilometerTo = trip.KilometerTo,
                Price = trip.Price,
                CarName = trip.CarName,
                CarNumber = trip.CarNumber,
                DriverName = trip.DriverName,
                DriverNumber = trip.DriverNumber,
                DriverBloodGroup = trip.DriverBloodGroup,
                DriverLicense = trip.DriverLicense,
                DriverAadharNumber = trip.DriverAadharNumber,
                CustomerName = trip.CustomerName,
                CustomerContact = trip.CustomerContact,
                Remark = trip.Remark,
                CreatedAt = trip.CreatedAt
            }).ToList();

            return Ok(new
            {
                TotalRecords = totalRecords,
                Page = page,
                PageSize = pageSize,
                Trips = tripsDto
            });
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrip(int id)
        {
            var trip = await dbContext.Trips.FindAsync(id);
            if (trip == null)
                return NotFound();

            var tripDto = new TripsDto
            {
                TripID = trip.TripID,
                PickUpLocation = trip.PickUpLocation,
                //PickUpDateTime = trip.PickUpDateTime,
                PickUpDate = trip.PickUpDate,
                PickUpTime = trip.PickUpTime,
                DropOffLocation = trip.DropOffLocation,
                DropOffDate = trip.DropOffDate,
                DropOffTime = trip.DropOffTime,
                //DropOffDateTime = trip.DropOffDateTime,
                KilometerFrom = trip.KilometerFrom,
                KilometerTo = trip.KilometerTo,
                Price = trip.Price,
                CarName = trip.CarName,
                CarNumber = trip.CarNumber,
                DriverName = trip.DriverName,
                DriverNumber = trip.DriverNumber,
                DriverBloodGroup = trip.DriverBloodGroup,
                DriverLicense = trip.DriverLicense,
                DriverAadharNumber = trip.DriverAadharNumber,
                CustomerName = trip.CustomerName,
                CustomerContact = trip.CustomerContact,
                Remark = trip.Remark,
                CreatedAt = trip.CreatedAt
            };

            return Ok(tripDto);
        }

        [HttpGet("generate-pdf/{id}")]
        public async Task<IActionResult> GenerateTripPdf(int id)
        {
            var trip = await dbContext.Trips.FindAsync(id);
            if (trip == null)
                return NotFound();

            // Create PDF document
            var pdfBytes = CreateTripPdf(trip);

            return File(pdfBytes, "application/pdf", $"Trip_{trip.TripID}.pdf");
        }

        private byte[] CreateTripPdf(Trips trip)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);

                    // Header
                    page.Header()
                        .AlignCenter()
                        //.Text($"Trip Details - ID: {trip.TripID}")
                        .Text($"Trip Details")
                        .FontSize(18)
                        .Bold();

                    page.Content().Column(column =>
                    {
                        // Customer Details Section
                        column.Item().Text("Customer Details").FontSize(14).Bold().Underline();
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(150);
                                columns.RelativeColumn();
                            });

                            table.Cell().Text("Customer Name:").Bold();
                            table.Cell().Text(trip.CustomerName);

                            table.Cell().Text("Customer Contact:").Bold();
                            table.Cell().Text(trip.CustomerContact);

                            table.Cell().Text("Pick-up Location:").Bold();
                            table.Cell().Text(trip.PickUpLocation);

                            table.Cell().Text("Pick-up Date:").Bold();
                            table.Cell().Text(trip.PickUpDate.ToString("yyyy-MM-dd"));

                            table.Cell().Text("Pick-up Time:").Bold();
                            table.Cell().Text(trip.PickUpTime);

                            table.Cell().Text("Drop-off Location:").Bold();
                            table.Cell().Text(trip.DropOffLocation);

                            table.Cell().Text("Drop-off Date:").Bold();
                            table.Cell().Text(trip.DropOffDate.ToString("yyyy-MM-dd"));

                            table.Cell().Text("Drop-off Time:").Bold();
                            table.Cell().Text(trip.DropOffTime);

                            table.Cell().Text("Remarks:").Bold();
                            table.Cell().Text(trip.Remark);
                        });

                        column.Item().PaddingVertical(10);

                        // Car Details Section
                        column.Item().Text("Car Details").FontSize(14).Bold().Underline();
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(150);
                                columns.RelativeColumn();
                            });

                            table.Cell().Text("Car Name:").Bold();
                            table.Cell().Text(trip.CarName);

                            table.Cell().Text("Car Number:").Bold();
                            table.Cell().Text(trip.CarNumber);

                            table.Cell().Text("Kilometers Travelled:").Bold();
                            table.Cell().Text($"{trip.KilometerFrom} - {trip.KilometerTo} km");

                            table.Cell().Text("Price:").Bold();
                            table.Cell().Text($"₹ {trip.Price}");
                        });

                        column.Item().PaddingVertical(10);

                        // Driver Details Section
                        column.Item().Text("Driver Details").FontSize(14).Bold().Underline();
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(150);
                                columns.RelativeColumn();
                            });

                            table.Cell().Text("Driver Name:").Bold();
                            table.Cell().Text(trip.DriverName);

                            table.Cell().Text("Driver Contact:").Bold();
                            table.Cell().Text(trip.DriverNumber);

                            table.Cell().Text("Driver Blood Group:").Bold();
                            table.Cell().Text(trip.DriverBloodGroup);

                            table.Cell().Text("Driver License:").Bold();
                            table.Cell().Text(trip.DriverLicense);

                            table.Cell().Text("Driver Aadhar Number:").Bold();
                            table.Cell().Text(trip.DriverAadharNumber);
                        });
                    });

                    // Footer
                    page.Footer()
                        .AlignCenter()
                        .Text("Generated by Pocketcar System")
                        .FontSize(10);
                });
            }).GeneratePdf();
        }

        [HttpPost]
        public async Task<IActionResult> AddTrip([FromBody] TripsDto tripDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var trip = new Trips
            {
                PickUpLocation = tripDto.PickUpLocation,
                PickUpDate = tripDto.PickUpDate,
                PickUpTime = tripDto.PickUpTime,
                //PickUpDateTime = tripDto.PickUpDateTime,
                DropOffDate = tripDto.DropOffDate,
                DropOffTime = tripDto.DropOffTime,
                DropOffLocation = tripDto.DropOffLocation,
                //DropOffDateTime = tripDto.DropOffDateTime,
                KilometerFrom = tripDto.KilometerFrom,
                KilometerTo = tripDto.KilometerTo,
                Price = tripDto.Price,
                CarName = tripDto.CarName,
                CarNumber = tripDto.CarNumber,
                DriverName = tripDto.DriverName,
                DriverNumber = tripDto.DriverNumber,
                DriverBloodGroup = tripDto.DriverBloodGroup,
                DriverLicense = tripDto.DriverLicense,
                DriverAadharNumber = tripDto.DriverAadharNumber,
                CustomerName = tripDto.CustomerName,
                CustomerContact = tripDto.CustomerContact,
                Remark = tripDto.Remark,
                CreatedAt = tripDto.CreatedAt
            };

            dbContext.Trips.Add(trip);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTrip), new { id = trip.TripID }, tripDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrip(int id, [FromBody] TripsDto updatedTripDto)
        {
            if (id != updatedTripDto.TripID)
                return BadRequest();

            var existingTrip = await dbContext.Trips.FindAsync(id);
            if (existingTrip == null)
                return NotFound();

            existingTrip.PickUpLocation = updatedTripDto.PickUpLocation;
            existingTrip.PickUpDate = updatedTripDto.PickUpDate;
            existingTrip.PickUpTime = updatedTripDto.PickUpTime;
            //existingTrip.PickUpDateTime = updatedTripDto.PickUpDateTime;
            existingTrip.DropOffLocation = updatedTripDto.DropOffLocation;
            existingTrip.DropOffDate = updatedTripDto.DropOffDate;
            existingTrip.DropOffTime = updatedTripDto.DropOffTime;
            //existingTrip.DropOffDateTime = updatedTripDto.DropOffDateTime;
            existingTrip.KilometerFrom = updatedTripDto.KilometerFrom;
            existingTrip.KilometerTo = updatedTripDto.KilometerTo;
            existingTrip.Price = updatedTripDto.Price;
            existingTrip.CarName = updatedTripDto.CarName;
            existingTrip.CarNumber = updatedTripDto.CarNumber;
            existingTrip.DriverName = updatedTripDto.DriverName;
            existingTrip.DriverNumber = updatedTripDto.DriverNumber;
            existingTrip.DriverBloodGroup = updatedTripDto.DriverBloodGroup;
            existingTrip.DriverLicense = updatedTripDto.DriverLicense;
            existingTrip.DriverAadharNumber = updatedTripDto.DriverAadharNumber;
            existingTrip.CustomerName = updatedTripDto.CustomerName;
            existingTrip.CustomerContact = updatedTripDto.CustomerContact;
            existingTrip.Remark = updatedTripDto.Remark;

            await dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrip(int id)
        {
            var trip = await dbContext.Trips.FindAsync(id);
            if (trip == null)
                return NotFound();

            dbContext.Trips.Remove(trip);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
