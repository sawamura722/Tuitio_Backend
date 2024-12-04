using Tuitio.DTOs;

namespace Tuitio.Services.IService
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync();
        Task<CategoryDTO> GetCategoryByIdAsync(int id);
        Task<CategoryDTO> CreateCategoryAsync(CategoryDTO categoryDTO);
        Task<bool> UpdateCategoryAsync(int id, CategoryDTO categoryDTO);
        Task<bool> DeleteCategoryAsync(int id);
        Task<bool> AddCategoryToCourseAsync(int courseId, int categoryId);
        Task<bool> RemoveCategoryFromCourseAsync(int courseId, int categoryId);
        Task<IEnumerable<CategoryDTO>> GetCourseCategoriesAsync(int courseId); 
    }


}
