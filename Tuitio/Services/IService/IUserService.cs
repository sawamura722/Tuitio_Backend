using Tuitio.DTOs;

namespace Tuitio.Services.IService
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> GetUserByIdAsync(int id);
        Task<UserDTO> UpdateUserAsync(int userId, UpdateUserDTO updateUserDTO);
    }
}
