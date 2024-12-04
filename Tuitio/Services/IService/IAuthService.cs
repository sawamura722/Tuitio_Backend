using Tuitio.DTOs;

namespace Tuitio.Services.IService
{
    public interface IAuthService
    {
        Task<string> Register(UserDTO userDto);
        Task<string> RegisterTeacher(UserDTO userDto);
        Task<string> Login(string email, string password);
    }
}
