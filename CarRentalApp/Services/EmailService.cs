using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using CarRentalApp.Models.Entities;
using System.Text.RegularExpressions;

namespace CarRentalApp.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(Email model)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");

            // Create SMTP client
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(emailSettings["SmtpServer"], int.Parse(emailSettings["Port"]), MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(emailSettings["SenderEmail"], emailSettings["SenderPassword"]);

                // 1️⃣ **Booking Confirmation Email to User**
                var userEmailMessage = new MimeMessage();
                userEmailMessage.From.Add(new MailboxAddress("Pocket Cars", emailSettings["SenderEmail"]));
                userEmailMessage.To.Add(new MailboxAddress(model.FullName, model.UserEmail));
                userEmailMessage.Subject = "Your Booking is Confirmed – Pocket Cars!";

                userEmailMessage.Body = new TextPart("html")
                {
                    Text = $@"
                <p>Dear {model.FullName},</p>
                <p>Thank you for choosing <strong>Pocket Cars</strong>! Your booking has been confirmed. Here are the details:</p>
                <p><strong>Pick-up Location:</strong> {model.PickupLocation}</p>
                <p><strong>Pick-up Date:</strong> {model.PickupDate}</p>
                <p><strong>Drop Location:</strong> {model.DropLocation}</p>
                <p><strong>Drop Date:</strong> {model.DropDate}</p>
                <p>For any changes or queries, feel free to contact us.</p>
                <p><strong>Safe travels!</strong><br>Pocket Cars Team</p>"
                };

                await client.SendAsync(userEmailMessage);

                // 2️⃣ **Booking Enquiry Email to Admin**
                var adminEmailMessage = new MimeMessage();
                adminEmailMessage.From.Add(new MailboxAddress("Pocket Cars", emailSettings["SenderEmail"]));
                adminEmailMessage.To.Add(new MailboxAddress("Admin", "zoroooo0930@gmail.com"));
                adminEmailMessage.Subject = "New Booking Enquiry – Pocket Cars";

                adminEmailMessage.Body = new TextPart("html")
                {
                    Text = $@"
                <p>Dear Admin,</p>
                <p>A new booking enquiry has been received. Here are the details:</p>
                <p><strong>Full Name:</strong> {model.FullName}</p>
                <p><strong>Contact:</strong> {model.ContactNumber}</p>
                <p><strong>Email:</strong> {model.UserEmail}</p>
                <p><strong>Pick-up Location:</strong> {model.PickupLocation}</p>
                <p><strong>Pick-up Date:</strong> {model.PickupDate}</p>
                <p><strong>Drop Location:</strong> {model.DropLocation}</p>
                <p><strong>Drop Date:</strong> {model.DropDate}</p>
                <p>Please review and take necessary action.</p>
                <p><strong>Best Regards,</strong><br>Pocket Cars System</p>"
                };

                await client.SendAsync(adminEmailMessage);

                // Disconnect SMTP client
                await client.DisconnectAsync(true);
            }
        }
    }
}
