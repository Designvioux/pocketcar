using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CarRentalApp.Services;
using CarRentalApp.Models.Entities;

namespace CarRentalApp.Controllers
{
    [Route("api/email")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly EmailService _emailService;

        public EmailController(EmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] Email model)
        {
            if (model == null || string.IsNullOrEmpty(model.UserEmail))
                return BadRequest("Invalid data");

            await _emailService.SendEmailAsync(model);
            return Ok(new { message = "Email Sent Successfully!" });
        }
    }
}