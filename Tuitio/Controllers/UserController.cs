using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tuitio.DTOs;
using Tuitio.Services.IService;

namespace Tuitio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }

        //[Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDTO>> UpdateUser(int id, [FromForm] UpdateUserDTO updateUserDTO)
        {
            var updatedUser = await _userService.UpdateUserAsync(id, updateUserDTO);
            if (updatedUser == null)
            {
                return NotFound("User not found");
            }

            return Ok(updatedUser);
        }
    }
}
