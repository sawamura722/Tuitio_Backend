using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tuitio.DTOs;
using Tuitio.Models;
using Tuitio.Services.IService;

namespace Tuitio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicController : ControllerBase
    {
        private readonly ITopicService _topicService;

        public TopicController(ITopicService topicService)
        {
            _topicService = topicService;
        }

        // GET: api/topic/course/{courseId}
        [HttpGet("course/{courseId}")]
        public async Task<ActionResult<IEnumerable<TopicDTO>>> GetTopicsByCourseId(int courseId)
        {
            var topics = await _topicService.GetTopicsByCourseIdAsync(courseId);
            // Assuming that the service returns a collection of Topic objects.
            var topicDTOs = topics.Select(topic => new TopicDTO
            {
                TopicId = topic.TopicId,
                CourseId = topic.CourseId,
                TopicTitle = topic.TopicTitle
            });

            return Ok(topicDTOs);
        }

        // GET: api/topic/{topicId}
        [HttpGet("{topicId}")]
        public async Task<ActionResult<TopicDTO>> GetTopicById(int topicId)
        {
            var topic = await _topicService.GetTopicByIdAsync(topicId);
            if (topic == null)
            {
                return NotFound();
            }

            return Ok(topic);
        }


        // POST: api/topic/course/{courseId}
        //[Authorize(Roles = "TEACHER")]
        [HttpPost("course/{courseId}")]
        public async Task<ActionResult<TopicDTO>> CreateTopic(int courseId, [FromBody] TopicDTO topicDTO)
        {
            if (topicDTO == null || string.IsNullOrEmpty(topicDTO.TopicTitle))
            {
                return BadRequest("Topic data is invalid.");
            }

            var createdTopic = await _topicService.CreateTopicAsync(courseId, topicDTO);
            var createdTopicDTO = new TopicDTO
            {
                TopicId = createdTopic.TopicId,
                CourseId = createdTopic.CourseId,
                TopicTitle = createdTopic.TopicTitle
            };

            return CreatedAtAction(nameof(GetTopicsByCourseId), new { courseId = courseId }, createdTopicDTO);
        }

        // PUT: api/topic/{id}
        [Authorize(Roles = "TEACHER")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTopic(int id, [FromBody] TopicDTO topicDTO)
        {
            if (topicDTO == null || string.IsNullOrEmpty(topicDTO.TopicTitle))
            {
                return BadRequest("Topic data is invalid.");
            }

            var updated = await _topicService.UpdateTopicAsync(id, topicDTO);
            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/topic/{id}
        [Authorize(Roles = "TEACHER")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTopic(int id)
        {
            var deleted = await _topicService.DeleteTopicAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        // GET: api/topic/{topicId}/duration
        [HttpGet("{topicId}/duration")]
        public async Task<ActionResult<TimeOnly>> GetTotalDurationByTopicId(int topicId)
        {
            try
            {
                var totalDuration = await _topicService.GetTotalDurationByTopicIdAsync(topicId);


                return Ok(totalDuration);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }


    }
}
