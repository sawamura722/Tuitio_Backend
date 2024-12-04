using Tuitio.DTOs;

namespace Tuitio.Services.IService
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseDTO>> GetAllCoursesAsync();
        Task<CourseDTO> GetCourseByIdAsync(int id);
        Task<IEnumerable<CourseDTO>> GetCoursesByTeacherIdAsync(int teacherId);
        Task<CourseDTO> CreateCourseAsync(CourseDTO courseDto);
        Task<bool> UpdateCourseAsync(int id, CourseDTO courseDto);
        Task<bool> DeleteCourseAsync(int id);
        Task<IEnumerable<CourseDTO>> GetCoursesWithCategoriesAsync(IEnumerable<int> categoryIds);
        Task<TimeOnly> GetCourseDurationAsync(int courseId);
    }

}
