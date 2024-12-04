using Tuitio.DTOs;

namespace Tuitio.Services.IService
{
    public interface IRegistrationService
    {
        Task<IEnumerable<RegistrationDTO>> GetAllRegistrationsAsync();
        Task<IEnumerable<RegistrationDTO>> GetRegistrationsByStudentIdAsync(int studentId);
        Task<RegistrationDTO> RegisterStudentInCourseAsync(int studentId, int courseId);
        Task<bool> UnregisterStudentFromCourseAsync(int studentId, int courseId);
        Task<IEnumerable<RegistrationDTO>> GetStudentsByCourseIdAsync(int courseId);
    }
}
