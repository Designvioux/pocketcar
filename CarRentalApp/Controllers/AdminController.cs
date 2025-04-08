using CarRentalApp.Data;
using CarRentalApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CarRentalApp.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly WheelzOnDbContext _dbContext;
        private readonly AuthService _authService;

        public AdminController(WheelzOnDbContext dbContext, AuthService authService)
        {
            _dbContext = dbContext;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var admin = await _dbContext.Admin.FirstOrDefaultAsync(a => a.Email == loginDto.Email);
                if (admin == null || admin.PasswordHash != loginDto.Password)
                {
                    return Unauthorized(new { message = "Invalid credentials" });
                }

                //var expiration = loginDto.KeepLoggedIn ? 30 : 1;
                var token = _authService.GenerateJwtToken(admin.Email, 1);

                return Ok(new { token });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                return StatusCode(500, new { message = "Something went wrong on the server." });
            }
        }

        [HttpGet("test-admin")]
        public async Task<IActionResult> TestAdmin()
        {
            try
            {
                var admin = await _dbContext.Admin.FirstOrDefaultAsync();
                if (admin == null) return NotFound("No admin found.");
                return Ok(admin);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"DB ERROR: {ex.Message}");
            }
        }
    }

    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        //public bool KeepLoggedIn { get; set; }
    }
}
