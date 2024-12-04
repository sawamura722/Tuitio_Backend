using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tuitio.DTOs;
using Tuitio.Models;
using Tuitio.Services.IService;

namespace Tuitio.Services.ServiceImpl
{
    public class CategoryService : ICategoryService
    {
        private readonly TutoringSchoolContext _context;
        private readonly IMapper _mapper;

        public CategoryService(TutoringSchoolContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
        }
        public async Task<CategoryDTO> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return null;
            return _mapper.Map<CategoryDTO>(category);
        }
        public async Task<CategoryDTO> CreateCategoryAsync(CategoryDTO categoryDTO)
        {
            var category = _mapper.Map<Category>(categoryDTO);
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return _mapper.Map<CategoryDTO>(category);
        }
        public async Task<bool> UpdateCategoryAsync(int id, CategoryDTO categoryDTO)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;

            category.CategoryName = categoryDTO.CategoryName;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Categories.Any(c => c.CategoryId == id))
                    return false;
                throw;
            }
        }
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddCategoryToCourseAsync(int courseId, int categoryId)
        {
            // Find the course and category by their IDs
            var course = await _context.Courses
                .Include(c => c.Categories)
                .FirstOrDefaultAsync(c => c.CourseId == courseId);
            var category = await _context.Categories.FindAsync(categoryId);

            // Check if the course and category exist
            if (course == null || category == null)
            {
                return false;
            }

            // Check if the course already contains the category
            if (course.Categories.Any(c => c.CategoryId == categoryId))
            {
                return false; // Category is already associated with this course
            }

            // Add the category to the course's categories list
            course.Categories.Add(category);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveCategoryFromCourseAsync(int courseId, int categoryId)
        {
            // Find the course including its categories
            var course = await _context.Courses
                .Include(c => c.Categories)
                .FirstOrDefaultAsync(c => c.CourseId == courseId);

            // Check if the course exists and the category is associated
            if (course == null || !course.Categories.Any(c => c.CategoryId == categoryId))
            {
                return false; // Course not found or category not associated
            }

            // Find the category to remove
            var categoryToRemove = course.Categories.First(c => c.CategoryId == categoryId);

            // Remove the category from the course's categories list
            course.Categories.Remove(categoryToRemove);
            await _context.SaveChangesAsync(); // Save changes to the database

            return true;
        }

        public async Task<IEnumerable<CategoryDTO>> GetCourseCategoriesAsync(int courseId)
        {
            // Find the course and include its categories
            var course = await _context.Courses
                .Include(c => c.Categories)
                .FirstOrDefaultAsync(c => c.CourseId == courseId);

            // Check if the course exists
            if (course == null)
            {
                return Enumerable.Empty<CategoryDTO>(); // Return an empty collection if course not found
            }

            // Map the categories to CategoryDTOs and return
            return _mapper.Map<IEnumerable<CategoryDTO>>(course.Categories);
        }

    }
}
