using Tuitio.DTOs;

namespace Tuitio.Services.IService
{
    public interface ILessonService
    {
        Task<IEnumerable<LessonDTO>> GetLessonsByTopicIdAsync(int courseId);
        Task<LessonDTO> GetLessonByIdAsync(int id);
        Task<LessonDTO> CreateLessonAsync(LessonDTO lessonDto);
        Task<bool> UpdateLessonAsync(int id, LessonDTO lessonDto);
        Task<bool> DeleteLessonAsync(int id);
    }


}
