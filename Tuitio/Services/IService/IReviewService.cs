using System.Collections.Generic;
using System.Threading.Tasks;
using Tuitio.DTOs;

namespace Tuitio.Services.IService
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewDTO>> GetReviewsByCourseIdAsync(int courseId);
        Task<ReviewDTO> AddReviewAsync(CreateReviewDTO reviewDto);
        Task<bool> DeleteReviewAsync(int reviewId);
    }
}
