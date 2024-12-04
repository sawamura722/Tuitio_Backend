using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tuitio.DTOs;
using Tuitio.Models;
using Tuitio.Services.IService;

namespace Tuitio.Services.ServiceImpl
{
    public class ReviewService : IReviewService
    {
        private readonly TutoringSchoolContext _context;
        private readonly IMapper _mapper;

        public ReviewService(TutoringSchoolContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReviewDTO>> GetReviewsByCourseIdAsync(int courseId)
        {
            var reviews = await _context.Reviews
                .Include(r => r.Student) // Include student to get all necessary details
                .Where(r => r.CourseId == courseId)
                .ToListAsync();

            // Use AutoMapper to map reviews to ReviewDTOs
            var reviewDTOs = _mapper.Map<IEnumerable<ReviewDTO>>(reviews);

            return reviewDTOs;
        }



        // Example mapping logic; adjust based on your actual code
        public async Task<ReviewDTO> AddReviewAsync(CreateReviewDTO reviewDto)
        {
            var review = _mapper.Map<Review>(reviewDto);
            review.CreatedAt = DateTime.Now;

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return _mapper.Map<ReviewDTO>(review);
        }

        public async Task<bool> DeleteReviewAsync(int reviewId)
        {
            var review = await _context.Reviews.FindAsync(reviewId);

            if (review == null) return false;

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
