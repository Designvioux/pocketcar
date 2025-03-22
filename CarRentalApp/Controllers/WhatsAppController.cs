using Microsoft.AspNetCore.Mvc;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using System;


namespace CarRentalApp.Controllers
{
    [Route("api/whatsapp")]
    [ApiController]
    public class WhatsAppController : Controller
    {
        private readonly string accountSid = "AC522f51d464433331a5e0e4d2770a9095";  // Twilio Account SID
        private readonly string authToken = "7962e2d0507534371683cd5fc2d09512";    // Twilio Auth Token
        private readonly string twilioWhatsAppNumber = "whatsapp:+14155238886"; // Twilio Sandbox Number
        private readonly string adminWhatsAppNumber = "whatsapp:+919561007745"; // Admin's WhatsApp Number

        public WhatsAppController()
        {
            TwilioClient.Init(accountSid, authToken);
        }

        [HttpPost("send-confirmation")]
        public IActionResult SendConfirmation([FromBody] BookingModel model)
        {
            try
            {
                var userMessage = $@"
                    Dear {model.FullName}, 
                    Thank you for choosing *Pocket Cars*! 🚗 Your booking has been confirmed. Here are the details:

                    📍 *Pick-up Location:* {model.PickupLocation}
                    📅 *Pick-up Date:* {model.PickupDate}

                    📍 *Drop Location:* {model.DropLocation}
                    📅 *Drop Date:* {model.DropDate}
        
                    For any changes or queries, feel free to contact us.
                    *Safe travels!* ✨ 
                    - Pocket Cars Team";

                var messageResult = MessageResource.Create(
                    from: new PhoneNumber(twilioWhatsAppNumber),
                    to: new PhoneNumber($"whatsapp:{model.ContactNumber}"), 
                    body: userMessage
                );

                return Ok(new { MessageId = messageResult.Sid, Status = "Message Sent to User!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("send-admin-notification")]
        public IActionResult SendAdminNotification([FromBody] BookingModel model)
        {
            try
            {
                var adminMessage = $@"
📩 *New Booking Enquiry Received* 📩

👤 *Full Name:* {model.FullName}
📞 *Contact:* {model.ContactNumber}
✉️ *Email:* {model.UserEmail}

📍 *Pick-up Location:* {model.PickupLocation}
📅 *Pick-up Date:* {model.PickupDate}

📍 *Drop Location:* {model.DropLocation}
📅 *Drop Date:* {model.DropDate}

🔔 *Please review and take necessary action!*";

                var messageResult = MessageResource.Create(
                    from: new PhoneNumber(twilioWhatsAppNumber),
                    to: new PhoneNumber(adminWhatsAppNumber), // Send to Admin's WhatsApp
                    body: adminMessage
                );

                return Ok(new { MessageId = messageResult.Sid, Status = "Message Sent to Admin!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }

    public class BookingModel
    {
        public string FullName { get; set; }
        public string ContactNumber { get; set; }
        public string UserEmail { get; set; }
        public string PickupLocation { get; set; }
        public string PickupDate { get; set; }
        public string DropLocation { get; set; }
        public string DropDate { get; set; }
    }
}

