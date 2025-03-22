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
            var admin = await _dbContext.Admin.FirstOrDefaultAsync(a => a.Email == loginDto.Email);
            if (admin == null || admin.PasswordHash != loginDto.Password) // ✅ Plain text check
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            var token = _authService.GenerateJwtToken(admin.Email);
            return Ok(new { token });
        }
    }

    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
