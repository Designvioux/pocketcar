using CarRentalApp.Data;
using CarRentalApp.Models.Entities;
using CarRentalApp.Models.EntityDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalApp.Controllers
{
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
    }
}
