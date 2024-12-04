using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tuitio.DTOs;
using Tuitio.Services;
using Tuitio.Services.IService;

namespace Tuitio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly ILessonService _lessonService;
        private readonly ILogger<LessonController> _logger;

        public LessonController(ILessonService lessonService, ILogger<LessonController> logger)
        {
            _lessonService = lessonService;
            _logger = logger;
        }

        // GET: api/Lesson/course/{courseId}
        [HttpGet("topic/{topicId}")]
        public async Task<ActionResult<IEnumerable<LessonDTO>>> GetLessonsByTopicId(int topicId)
        {
            var lessons = await _lessonService.GetLessonsByTopicIdAsync(topicId);

            return Ok(lessons);
        }

        // GET: api/Lesson/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<LessonDTO>> GetLesson(int id)
        {
            var lesson = await _lessonService.GetLessonByIdAsync(id);

            return Ok(lesson);
        }

        [Authorize(Roles = "TEACHER")]
        [HttpPost]
        public async Task<ActionResult<LessonDTO>> CreateLesson([FromForm] LessonDTO lessonDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newLesson = await _lessonService.CreateLessonAsync(lessonDto);
                return CreatedAtAction(nameof(GetLesson), new { id = newLesson.LessonId }, newLesson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating lesson");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Roles = "TEACHER")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLesson(int id, [FromForm] LessonDTO lessonDto)
        {
            if (id != lessonDto.LessonId)
            {
                return BadRequest("Lesson ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedLesson = await _lessonService.UpdateLessonAsync(id, lessonDto);
                if (updatedLesson == null)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating lesson");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Roles = "TEACHER")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            try
            {
                var deleted = await _lessonService.DeleteLessonAsync(id);
                if (!deleted)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting lesson");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
