using Tuitio.DTOs;
using Tuitio.Models;

namespace Tuitio.Services.IService
{
    public interface ITopicService
    {
        Task<IEnumerable<TopicDTO>> GetTopicsByCourseIdAsync(int id);
        Task<TopicDTO> GetTopicByIdAsync(int topicId);
        Task<TopicDTO> CreateTopicAsync(int courseId, TopicDTO topicDTO);
        Task<bool> UpdateTopicAsync(int id, TopicDTO topicDTO);
        Task<bool> DeleteTopicAsync(int id);
        Task<TimeOnly> GetTotalDurationByTopicIdAsync(int topicId);
    }
}
