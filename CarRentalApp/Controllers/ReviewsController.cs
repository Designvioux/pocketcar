using CarRentalApp.Data;
using CarRentalApp.Models.Entities;
using CarRentalApp.Models.EntityDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly WheelzOnDbContext dbContext;

        public ReviewsController(WheelzOnDbContext DbContext)
        {
            dbContext = DbContext;
        }

        [HttpPut]
        [Route("{id:int}")]

        public IActionResult AddReview(int id, ReviewsDto reviewsDto)
        {
            var review = new Review()
            {
                UserId = reviewsDto.UserId,
                User = reviewsDto.User,
                CarId = reviewsDto.CarId,
                Car = reviewsDto.Car,
                Rating = reviewsDto.Rating,
                Comment = reviewsDto.Comment,
                ReviewDate = reviewsDto.ReviewDate
            };
            dbContext.Reviews.Add(review);
            dbContext.SaveChanges();

            return Ok(review);
        }

        [HttpGet]
        public IActionResult GetReview()
        {
            var allReview = dbContext.Reviews.ToList();

            return Ok(allReview);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetReviewById(int id)
        {
            var review = dbContext.Reviews.Find(id);
            if (id == null)
            {
                return NotFound();
            }

            return Ok(review);
        }

        [HttpDelete]
        [Route("{id:int}")]

        public IActionResult DeleteReview(int id)
        {
            var review = dbContext.Reviews.Find(id);
            if(id == null)
            {
                return NotFound();
            }

            dbContext.Reviews.Remove(review);
            dbContext.SaveChanges();

            return Ok(review);

        }
    }
}
