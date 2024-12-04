using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tuitio.DTOs;
using Tuitio.Models;
using Tuitio.Services.IService;

namespace Tuitio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCategory(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory([FromForm] CategoryDTO categoryDTO)
        {
            var createdCategory = await _categoryService.CreateCategoryAsync(categoryDTO);
            return CreatedAtAction(nameof(GetCategories), new { id = createdCategory.CategoryId }, createdCategory);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, [FromForm] CategoryDTO categoryDTO)
        {
            if (await _categoryService.UpdateCategoryAsync(id, categoryDTO))
            {
                return NoContent();
            }
            return BadRequest();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (await _categoryService.DeleteCategoryAsync(id))
            {
                return NoContent();
            }
            return BadRequest();
        }

        [Authorize(Roles = "TEACHER")]
        [HttpPost("{categoryId}/courses/{courseId}")]
        public async Task<IActionResult> AddCategoryToCourse(int categoryId, int courseId)
        {
            var result = await _categoryService.AddCategoryToCourseAsync(courseId, categoryId);
            if (!result)
            {
                return BadRequest("Either the course or category does not exist, or the category is already associated with this course.");
            }

            return Ok("Category added to the course successfully.");
        }

        [Authorize(Roles = "TEACHER")]
        [HttpDelete("{categoryId}/courses/{courseId}")]
        public async Task<IActionResult> RemoveCategoryFromCourse(int categoryId, int courseId)
        {
            var result = await _categoryService.RemoveCategoryFromCourseAsync(courseId, categoryId);
            if (!result)
            {
                return BadRequest("Either the course or category does not exist, or the category is not associated with this course.");
            }

            return Ok("Category removed from the course successfully.");
        }

        [HttpGet("courses/{courseId}/categories")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCourseCategories(int courseId)
        {
            var categories = await _categoryService.GetCourseCategoriesAsync(courseId);

            return Ok(categories);
        }

    }
}
