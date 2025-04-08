using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CarRentalApp.Models.Entities;
using CarRentalApp.Data;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class BookingController : ControllerBase
{

    private readonly WheelzOnDbContext dbContext;
    private readonly SmsService _smsService;

    public BookingController(WheelzOnDbContext DbContext, SmsService smsService)
    {
        dbContext = DbContext;
        _smsService = smsService;
    }


    /*[HttpPost("create")]
    public async Task<IActionResult> CreateBooking([FromBody] Booking request)
    {
        if (string.IsNullOrEmpty(request.FullName) || string.IsNullOrEmpty(request.ContactNumber))
        {
            return BadRequest("Full name and contact number are required.");
        }

        dbContext.Bookings.Add(request);
        await dbContext.SaveChangesAsync();

        string message = $"Booking Confirmation: {request.FullName}, Pickup: {request.PickupLocation} on {request.PickupDate}, Drop: {request.DropLocation} on {request.DropDate}.";
        await _smsService.SendSmsAsync(request.ContactNumber, message);

        return Ok(new { status = "Success", message = "Booking saved successfully!" });
    }*/


    [HttpPost("create")]
    public async Task<IActionResult> CreateBooking([FromBody] Booking request)
    {
        if (string.IsNullOrEmpty(request.FullName) || string.IsNullOrEmpty(request.ContactNumber))
        {
            return BadRequest("Full name and contact number are required.");
        }

        dbContext.Bookings.Add(request);
        await dbContext.SaveChangesAsync();

        // Create a new notification
        var notification = new Notification
        {
            Header ="New Booking Request Received for Upcoming Trip",
            Content = $"You have received a new booking request for a trip scheduled on <b>{request.PickupDate:dd MMM yyyy}</b>, <b>{request.PickupTime}</b>. " +
          $"The passenger, <b>{request.FullName}</b>, has requested transportation from <b>{request.PickupLocation}</b> to <b>{request.DropLocation}</b>.",
            CreatedAt = DateTime.UtcNow
        };

        dbContext.Notifications.Add(notification);
        await dbContext.SaveChangesAsync();

        return Ok(new { status = "Success", message = "Booking saved successfully!" });
    }


    [HttpGet("notifications")]
    public async Task<IActionResult> GetNotifications()
    {
        var notifications = await dbContext.Notifications.OrderByDescending(n => n.CreatedAt).ToListAsync();
        return Ok(notifications);
    }

    [HttpDelete("notifications/delete")]
    public async Task<IActionResult> DeleteNotifications([FromBody] int[] notificationIds)
    {
        var notifications = dbContext.Notifications.Where(n => notificationIds.Contains(n.Id));
        dbContext.Notifications.RemoveRange(notifications);
        await dbContext.SaveChangesAsync();
        return Ok(new { status = "Success", message = "Notifications deleted successfully!" });
    }


    /*[HttpGet("all")]
    public async Task<ActionResult<IEnumerable<Booking>>> GetAllBookings()
    {
        var bookings = await dbContext.Bookings.ToListAsync();
        return Ok(bookings);
    }*/

    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<Booking>>> GetAllBookings(
    [FromQuery] string? customerName,
    [FromQuery] string? phoneNo,
    [FromQuery] string? cityPickUp,
    [FromQuery] string? cityDropOff,
    [FromQuery] DateTime? pickupDate,
    [FromQuery] TimeSpan? pickupTime,
    [FromQuery] DateTime? dropOffDate,
    [FromQuery] TimeSpan? dropOffTime,
    [FromQuery] string? selectedCar,
    [FromQuery]string? status,
    [FromQuery] string? tripType,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)

    {
        var query = dbContext.Bookings.AsQueryable();

        if (!string.IsNullOrEmpty(customerName))
            query = query.Where(b => b.FullName.Contains(customerName));

        if (!string.IsNullOrEmpty(phoneNo))
            query = query.Where(b => b.ContactNumber.Contains(phoneNo));

        if (!string.IsNullOrEmpty(cityPickUp))
            query = query.Where(b => b.PickupLocation.Contains(cityPickUp));

        if (!string.IsNullOrEmpty(cityDropOff))
            query = query.Where(b => b.DropLocation.Contains(cityDropOff));

        if (pickupDate.HasValue)
            query = query.Where(b => b.PickupDate.Date == pickupDate.Value.Date);

        if (pickupTime.HasValue)
            query = query.Where(b => b.PickupTime == pickupTime.Value);

        if (dropOffDate.HasValue)
            query = query.Where(b => b.DropDate.Date == dropOffDate.Value.Date);

        if (dropOffTime.HasValue)
            query = query.Where(b => b.DropOffTime == pickupTime.Value);

        if (!string.IsNullOrEmpty(selectedCar))
            query = query.Where(b => b.SelectedCar.Contains(selectedCar));

        if (!string.IsNullOrEmpty(status)) 
            query = query.Where(b => b.Status == status);

        if (!string.IsNullOrEmpty(tripType)) 
            query = query.Where(b => b.TripType == tripType);

        int totalRecords = await query.CountAsync();

        var bookings = await query.OrderByDescending(b => b.Id)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

        return Ok(new
        {
            TotalRecords = totalRecords,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
            Data = bookings
        });
    }


    [HttpPut("update-status/{id}")]
    public async Task<IActionResult> UpdateBookingStatus(int id, [FromBody] BookingStatusUpdate request)
    {
        var booking = await dbContext.Bookings.FindAsync(id);
        if (booking == null)
        {
            return NotFound("Booking not found");
        }

        if (string.IsNullOrEmpty(request.Status))
        {
            return BadRequest("Status is required.");
        }

        booking.Status = request.Status; 
        await dbContext.SaveChangesAsync();

        return Ok(new { message = "Booking status updated successfully" });
    }

    public class BookingStatusUpdate
    {
        public string Status { get; set; }
    }

    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok("BookingController is working!");
    }
}








/*private readonly SmsService _smsService;

public BookingController()
{
    _smsService = new SmsService();
}

[HttpPost("send-sms")]
public async Task<IActionResult> SendSms([FromBody] BookingRequest request)
{
    if (string.IsNullOrEmpty(request.FullName) || string.IsNullOrEmpty(request.ContactNumber))
    {
        return BadRequest("Full name and contact number are required.");
    }

    string message = $"Booking Confirmation: {request.FullName}, Pickup: {request.PickupLocation} on {request.PickupDate}, Drop: {request.DropLocation} on {request.DropDate}.";
    var response = await _smsService.SendSmsAsync(request.ContactNumber, message);

    return Ok(new { status = "Success", response });
}*/

/*private readonly SmsService _smsService;

    public BookingController(SmsService smsService)
    {
        _smsService = smsService;
    }

    [HttpPost("send-sms")]
    public async Task<IActionResult> SendSms([FromBody] Booking request)
    {
        if (string.IsNullOrEmpty(request.FullName) || string.IsNullOrEmpty(request.ContactNumber))
        {
            return BadRequest("Full name and contact number are required.");
        }

        string message = $"Booking Confirmation: {request.FullName}, Pickup: {request.PickupLocation} on {request.PickupDate}, Drop: {request.DropLocation} on {request.DropDate}.";

        var response = await _smsService.SendSmsAsync(request.ContactNumber, message);
        return Ok(new { status = "Success", response });
    }
}*/

/*public class BookingRequest
{
    public string FullName { get; set; }
    public string ContactNumber { get; set; }
    public string PickupLocation { get; set; }
    public string PickupDate { get; set; }
    public string DropLocation { get; set; }
    public string DropDate { get; set; }
}*/


