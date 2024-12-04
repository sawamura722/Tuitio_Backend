using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tuitio.DTOs;
using Tuitio.Services.IService;

namespace Tuitio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetReviewsByCourse(int courseId)
        {
            var reviews = await _reviewService.GetReviewsByCourseIdAsync(courseId);
            return Ok(reviews);
        }

        [Authorize(Roles = "USER")]
        [HttpPost]
        public async Task<IActionResult> AddReview([FromBody] CreateReviewDTO reviewDto)
        {
            var createdReview = await _reviewService.AddReviewAsync(reviewDto);
            return CreatedAtAction(nameof(GetReviewsByCourse), new { courseId = createdReview.CourseId }, createdReview);
        }


        [Authorize(Roles = "USER")]
        [HttpDelete("{reviewId}")]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            var result = await _reviewService.DeleteReviewAsync(reviewId);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
