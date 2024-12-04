using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tuitio.DTOs;
using Tuitio.Models;
using Tuitio.Services.IService;

namespace Tuitio.Services.ServiceImpl
{
    public class TopicService : ITopicService
    {
        private readonly TutoringSchoolContext _context;
        private readonly IMapper _mapper;

        public TopicService(TutoringSchoolContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<TopicDTO>> GetTopicsByCourseIdAsync(int courseId)
        {
            var topics = await _context.Topics
           .Where(t => t.CourseId == courseId)
           .ToListAsync();

            return _mapper.Map<IEnumerable<TopicDTO>>(topics);
        }

        public async Task<TopicDTO> GetTopicByIdAsync(int topicId)
        {
            var topic = await _context.Topics.FindAsync(topicId);
            if (topic == null)
            {
                return null; // Topic not found
            }
            return _mapper.Map<TopicDTO>(topic);
        }


        public async Task<TopicDTO> CreateTopicAsync(int courseId, TopicDTO topicDTO)
        {
            var topic = _mapper.Map<Topic>(topicDTO);
            _context.Topics.Add(topic);
            await _context.SaveChangesAsync();

            return _mapper.Map<TopicDTO>(topic);
        }

        public async Task<bool> UpdateTopicAsync(int id, TopicDTO topicDTO)
        {
            var topic = await _context.Topics.FindAsync(id);
            if (topic == null)
            {
                return false; // Topic not found
            }

            topic.TopicTitle = topicDTO.TopicTitle;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Topics.Any(t => t.TopicId == id))
                    return false;
                throw;
            }
        }

        public async Task<bool> DeleteTopicAsync(int id)
        {
            // Find the topic
            var topic = await _context.Topics
                .Include(t => t.Lessons) // Include the associated lessons
                .FirstOrDefaultAsync(t => t.TopicId == id);

            if (topic == null)
            {
                return false; // Topic not found
            }

            // Remove all associated lessons
            if (topic.Lessons != null && topic.Lessons.Any())
            {
                _context.Lessons.RemoveRange(topic.Lessons);
            }

            // Remove the topic
            _context.Topics.Remove(topic);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<TimeOnly> GetTotalDurationByTopicIdAsync(int topicId)
        {
            var totalDuration = new TimeSpan(); // Initialize a TimeSpan to hold total duration

            // Fetch the topic along with its lessons
            var topic = await _context.Topics
                .Include(t => t.Lessons)
                .FirstOrDefaultAsync(t => t.TopicId == topicId);

            // Check if there are lessons
            if (!topic.Lessons.Any())
            {
                // Log or return a specific message indicating no lessons
                return TimeOnly.FromTimeSpan(TimeSpan.Zero); // Return 00:00 if no lessons exist
            }

            // Iterate through lessons and sum up the durations
            foreach (var lesson in topic.Lessons)
            {
                if (lesson.Duration.HasValue) // Ensure the duration is not null
                {
                    totalDuration = totalDuration.Add(lesson.Duration.Value.ToTimeSpan());
                }
            }

            // Convert the total duration back to TimeOnly
            return TimeOnly.FromTimeSpan(totalDuration);
        }



    }
}
