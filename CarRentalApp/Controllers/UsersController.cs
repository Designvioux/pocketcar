using CarRentalApp.Data;
using CarRentalApp.Models.Entities;
using CarRentalApp.Models.EntityDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApp.Controllers
{
    /*[Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly WheelzOnDbContext dbContext;

        public UsersController(WheelzOnDbContext DbContext)
        {
            dbContext = DbContext;
        }


        [HttpGet]
        public IActionResult GetAllusers()
        {
            var allUsers = dbContext.Users.ToList();

            return Ok(allUsers);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetUserById(int id)
        {
            var user = dbContext.Users.Find(id);

            if (user == null)
            {
                return NotFound("Couldn't Find User");
            }

            return Ok(user);
        }

        [HttpPost]
        public IActionResult AddUsers(UserDto userDto)
        {
            var userEntity = new User()
            {
                FullName = userDto.FullName,
                Email = userDto.Email,
                PasswordHash = userDto.PasswordHash,
                Phone = userDto.Phone,
                Role = userDto.Role
            };

            dbContext.Users.Add(userEntity);
            dbContext.SaveChanges();

            return Ok(userEntity);
        }

        [HttpPut]
        [Route("{id:int}")]

        public IActionResult UpdateUser(int id, UserDto userDto)
        {
            var user = dbContext.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            user.FullName = userDto.FullName;
            user.Email = userDto.Email;
            user.PasswordHash = userDto.PasswordHash;
            user.Phone = userDto.Phone;
            user.Role = userDto.Role;

            dbContext.SaveChanges();
            return Ok(user);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult DeleteUser(int id)
        {
            var user = dbContext.Users.Find(id);

            if (user == null)
            {
                return NotFound();
            }

            dbContext.Users.Remove(user);
            dbContext.SaveChanges();

            return Ok();
        }
    }*/




    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly WheelzOnDbContext dbContext;

        public UsersController(WheelzOnDbContext DbContext)
        {
            dbContext = DbContext;
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var allUsers = dbContext.Users.Where(u => !u.IsDeleted).ToList();
            return Ok(allUsers);
        }

        [HttpGet("latest-user")]
        public IActionResult GetLatestUser()
        {
            var latestUser = dbContext.Users
                .Where(u => !u.IsDeleted) 
                .OrderByDescending(u => u.UserId) 
                .FirstOrDefault();

            if (latestUser == null)
            {
                return NotFound("No active user found.");
            }

            return Ok(latestUser);
        }


        [HttpGet("{id:int}")]
        public IActionResult GetUserById(int id)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            return Ok(user);
        }

        /*[HttpPost]
        public IActionResult AddUser([FromBody] UserDto userDto)
        {
            var userEntity = new User
            {
                FirstName = userDto.FirstName,
                Surname = userDto.Surname,
                DateOfBirth = userDto.DateOfBirth,
                PhoneNumber = userDto.PhoneNumber,
                Email = userDto.Email,
                Gender = userDto.Gender,
                City = userDto.City,
                ProfilePictureUrl = userDto.ProfilePictureUrl
            };

            dbContext.Users.Add(userEntity);
            dbContext.SaveChanges();

            return Ok(userEntity);
        }*/

        [HttpPost]
        public IActionResult AddUser([FromForm] UserDto userDto)
        {
            string profilePictureUrl = null;

            // Save the file if it exists
            if (userDto.ProfilePicture != null)
            {
                var filePath = Path.Combine("wwwroot/uploads", userDto.ProfilePicture.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    userDto.ProfilePicture.CopyTo(stream);
                }

                profilePictureUrl = $"/uploads/{userDto.ProfilePicture.FileName}";
            }

            var userEntity = new User
            {
                FirstName = userDto.FirstName,
                Surname = userDto.Surname,
                DateOfBirth = userDto.DateOfBirth ?? DateTime.Now,
                PhoneNumber = userDto.PhoneNumber,
                Email = userDto.Email,
                Gender = userDto.Gender,
                City = userDto.City,
                ProfilePictureUrl = profilePictureUrl
            };

            dbContext.Users.Add(userEntity);
            dbContext.SaveChanges();

            return Ok(userEntity);
        }

        /*[HttpPut("{id:int}")]
        public IActionResult UpdateUser(int id, [FromBody] UserDto userDto)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.FirstName = userDto.FirstName;
            user.Surname = userDto.Surname;
            user.DateOfBirth = userDto.DateOfBirth;
            user.PhoneNumber = userDto.PhoneNumber;
            user.Email = userDto.Email;
            user.Gender = userDto.Gender;
            user.City = userDto.City;
            //user.ProfilePictureUrl = userDto.ProfilePictureUrl;

            dbContext.SaveChanges();
            return Ok(user);
        }*/

        /*[HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateUser(int id, [FromForm] UserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest("Invalid user data.");
            }

            var user = dbContext.Users.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Update only non-null fields
            user.FirstName = string.IsNullOrWhiteSpace(userDto.FirstName) ? user.FirstName : userDto.FirstName;
            user.Surname = string.IsNullOrWhiteSpace(userDto.Surname) ? user.Surname : userDto.Surname;
            user.DateOfBirth = userDto.DateOfBirth ?? user.DateOfBirth;
            user.PhoneNumber = string.IsNullOrWhiteSpace(userDto.PhoneNumber) ? user.PhoneNumber : userDto.PhoneNumber;
            user.Email = string.IsNullOrWhiteSpace(userDto.Email) ? user.Email : userDto.Email;
            user.Gender = string.IsNullOrWhiteSpace(userDto.Gender) ? user.Gender : userDto.Gender;
            user.City = string.IsNullOrWhiteSpace(userDto.City) ? user.City : userDto.City;

            if (userDto.ProfilePicture != null && userDto.ProfilePicture.Length > 0)
            {
                // Handle profile picture upload logic
            }

            try
            {
                await dbContext.SaveChangesAsync();
                return Ok(new { Success = true, Message = "User updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }*/

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateUser(int id, [FromForm] UserDto userDto, IFormFile? profilePicture)
        {
            if (userDto == null)
                return BadRequest("Invalid user data.");

            var user = dbContext.Users.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
            if (user == null)
                return NotFound("User not found.");

            // Update user details
            user.FirstName = string.IsNullOrWhiteSpace(userDto.FirstName) ? user.FirstName : userDto.FirstName;
            user.Surname = string.IsNullOrWhiteSpace(userDto.Surname) ? user.Surname : userDto.Surname;
            user.DateOfBirth = userDto.DateOfBirth ?? user.DateOfBirth;
            user.PhoneNumber = string.IsNullOrWhiteSpace(userDto.PhoneNumber) ? user.PhoneNumber : userDto.PhoneNumber;
            user.Email = string.IsNullOrWhiteSpace(userDto.Email) ? user.Email : userDto.Email;
            user.Gender = string.IsNullOrWhiteSpace(userDto.Gender) ? user.Gender : userDto.Gender;
            user.City = string.IsNullOrWhiteSpace(userDto.City) ? user.City : userDto.City;

            // Handle profile picture upload
            if (profilePicture != null && profilePicture.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(profilePicture.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                    return BadRequest("Invalid file type. Only JPG, JPEG, PNG, and GIF are allowed.");

                if (profilePicture.Length > 2 * 1024 * 1024)
                    return BadRequest("File size exceeds the 2MB limit.");

                var fileName = Guid.NewGuid().ToString() + fileExtension;
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                var filePath = Path.Combine(uploadsFolder, fileName);

                Directory.CreateDirectory(uploadsFolder);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await profilePicture.CopyToAsync(stream);
                }

                user.ProfilePictureUrl = $"/uploads/{fileName}";
            }

            try
            {
                await dbContext.SaveChangesAsync();
                return Ok(new
                {
                    Success = true,
                    Message = "User updated successfully.",
                    UpdatedUser = new
                    {
                        user.FirstName,
                        user.Surname,
                        user.DateOfBirth,
                        user.PhoneNumber,
                        user.Email,
                        user.Gender,
                        user.City,
                        user.ProfilePictureUrl
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteUser(int id)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.IsDeleted = true;
            dbContext.SaveChanges();

            return Ok("User deleted successfully.");
        }
    }
}
