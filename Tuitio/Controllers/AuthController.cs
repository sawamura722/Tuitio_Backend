using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tuitio.DTOs;
using Tuitio.Services.IService;

namespace TCGCardCapital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDto)
        {
            if (userDto == null)
                return BadRequest(new { message = "Invalid user data" });

            try
            {
                var result = await _authService.Register(userDto);
                if (result == "User registered successfully")
                    return Ok(new { message = result });

                return BadRequest(new { message = result });
            }
            catch (Exception ex)
            {
                // Log exception (consider using a logging framework)
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("register/teacher")]
        public async Task<IActionResult> RegisterTeacher([FromBody] UserDTO userDto)
        {
            if (userDto == null)
                return BadRequest(new { message = "Invalid user data" });

            try
            {
                var result = await _authService.RegisterTeacher(userDto);
                if (result == "Teacher registered successfully")
                    return Ok(new { message = result });

                return BadRequest(new { message = result });
            }
            catch (Exception ex)
            {
                // Log exception (consider using a logging framework)
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var token = await _authService.Login(loginDto.Username, loginDto.Password);
            if (token == "Invalid credentials")
                return Unauthorized(token);
            return Ok(new { Token = token });
        }

    }
}
