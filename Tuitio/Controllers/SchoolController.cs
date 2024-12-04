using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tuitio.Models;
using Tuitio.Services.IService;

namespace Tuitio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private readonly ISchoolService _schoolService;

        public SchoolController(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }

        // GET: api/School
        [HttpGet]
        public async Task<ActionResult<IEnumerable<School>>> GetAllSchools()
        {
            var schools = await _schoolService.GetAllSchoolsAsync();
            return Ok(schools);
        }

        // GET: api/School/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<School>> GetSchoolById(int id)
        {
            var school = await _schoolService.GetSchoolByIdAsync(id);
            if (school == null)
            {
                return NotFound();
            }

            return Ok(school);
        }

        // POST: api/School
        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<ActionResult<School>> CreateSchool([FromBody] School school)
        {
            if (school == null)
            {
                return BadRequest("Invalid school data.");
            }

            var createdSchool = await _schoolService.CreateSchoolAsync(school);
            return CreatedAtAction(nameof(GetSchoolById), new { id = createdSchool.SchoolId }, createdSchool);
        }

        // PUT: api/School/{id}
        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSchool(int id, [FromBody] School school)
        {
            if (id != school.SchoolId)
            {
                return BadRequest("School ID mismatch.");
            }

            var updated = await _schoolService.UpdateSchoolAsync(id, school);
            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/School/{id}
        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchool(int id)
        {
            var deleted = await _schoolService.DeleteSchoolAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
