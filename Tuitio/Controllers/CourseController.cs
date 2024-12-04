using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tuitio.DTOs;
using Tuitio.Models;
using Tuitio.Services;
using Tuitio.Services.IService;

namespace Tuitio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly ILogger<CourseController> _logger;

        public CourseController(ICourseService courseService, ILogger<CourseController> logger)
        {
            _courseService = courseService;
            _logger = logger;
        }

        // GET: api/Course
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDTO>>> GetCourses()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            return Ok(courses);
        }

        // GET: api/Course/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDTO>> GetCourse(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }

        [Authorize(Roles = "TEACHER")]
        [HttpGet("teacher/{teacherId}")]
        public async Task<IActionResult> GetCoursesByTeacherId(int teacherId)
        {
            var courses = await _courseService.GetCoursesByTeacherIdAsync(teacherId);
     
            return Ok(courses);
        }


        [Authorize(Roles = "TEACHER")]
        [HttpPost]
        public async Task<ActionResult<CourseDTO>> CreateCourse([FromForm] CourseDTO courseDto)
        {
  
            try
            {
                var newCourse = await _courseService.CreateCourseAsync(courseDto);
                return CreatedAtAction(nameof(GetCourse), new { id = newCourse.CourseId }, newCourse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating course with Title: {CourseTitle}. Details: {Details}", courseDto.CourseTitle, ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Roles = "TEACHER")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromForm] CourseDTO courseDto)
        {

            try
            {
                var updatedCourse = await _courseService.UpdateCourseAsync(id, courseDto);
                if (updatedCourse == null)
                {
                    return NotFound();
                }
                return Ok(updatedCourse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating course");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Roles = "TEACHER")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            try
            {
                var deleted = await _courseService.DeleteCourseAsync(id);
                if (!deleted)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting course");
                return StatusCode(500, "Internal server error");
            }
        }

        // Update this method in CourseController
        [HttpGet("WithCategories")]
        public async Task<IActionResult> GetCoursesWithCategories([FromQuery] IEnumerable<int> categoryIds)
        {
            var courses = await _courseService.GetCoursesWithCategoriesAsync(categoryIds);
            return Ok(courses);
        }

        // GET: api/course/{courseId}/duration
        [HttpGet("{courseId}/duration")]
        public async Task<ActionResult<TimeOnly>> GetCourseDuration(int courseId)
        {
            var duration = await _courseService.GetCourseDurationAsync(courseId);

            return Ok(duration);
        }

    }
}
