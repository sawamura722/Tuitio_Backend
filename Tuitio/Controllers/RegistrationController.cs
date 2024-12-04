using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tuitio.DTOs;
using Tuitio.Services.IService;

namespace Tuitio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;

        public RegistrationController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        [HttpGet("allregistrations")]
        public async Task<ActionResult<IEnumerable<RegistrationDTO>>> GetAllRegistrations()
        {
            var registrations = await _registrationService.GetAllRegistrationsAsync();
            if (registrations == null)
            {
                return NotFound();
            }
            return Ok(registrations);
        }

        [Authorize(Roles = "USER,TEACHER,ADMIN")]
        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<IEnumerable<RegistrationDTO>>> GetRegistrationsByStudentId(int studentId)
        {
            var registrations = await _registrationService.GetRegistrationsByStudentIdAsync(studentId);
            if (registrations == null)
            {
                return NotFound();
            }
            return Ok(registrations);
        }

        //for walk-in customer
        [Authorize(Roles = "ADMIN")]
        [HttpPost("register")]
        public async Task<ActionResult<RegistrationDTO>> RegisterStudentInCourse(int studentId, int courseId)
        {
            try
            {
                var registration = await _registrationService.RegisterStudentInCourseAsync(studentId, courseId);
                return CreatedAtAction(nameof(GetRegistrationsByStudentId), new { studentId }, registration);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "USER,ADMIN")]
        [HttpDelete("unregister")]
        public async Task<IActionResult> UnregisterStudentFromCourse(int studentId, int courseId)
        {
            var result = await _registrationService.UnregisterStudentFromCourseAsync(studentId, courseId);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("course/{courseId}")]
        public async Task<ActionResult<IEnumerable<RegistrationDTO>>> GetStudentsByCourseId(int courseId)
        {
            var registrations = await _registrationService.GetStudentsByCourseIdAsync(courseId);
            if (registrations == null)
            {
                return NotFound();
            }

            return Ok(registrations);
        }
    }
}
