using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Tuitio.DTOs;
using Tuitio.Models;
using Tuitio.Services.IService;

namespace Tuitio.Services.ServiceImpl
{
    public class CourseService : ICourseService
    {
        private readonly TutoringSchoolContext _context;
        private readonly IMapper _mapper;

        public CourseService(TutoringSchoolContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CourseDTO>> GetAllCoursesAsync()
        {
            var courses = await _context.Courses.ToListAsync();
            return _mapper.Map<IEnumerable<CourseDTO>>(courses);
        }

        public async Task<CourseDTO> GetCourseByIdAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return null;
            return _mapper.Map<CourseDTO>(course);
        }
        public async Task<IEnumerable<CourseDTO>> GetCoursesByTeacherIdAsync(int teacherId)
        {
            var courses = await _context.Courses
                .Where(c => c.TeacherId == teacherId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<CourseDTO>>(courses);
        }

        public async Task<CourseDTO> CreateCourseAsync(CourseDTO courseDto)
        {
            // Check for overlapping courses
            if (await IsOverlappingCourseAsync(courseDto, null))
            {
                throw new Exception("Cannot create course: Overlaps with an existing course.");
            }

            if (courseDto.Image != null)
            {
                var imageFileName = Guid.NewGuid() + Path.GetExtension(courseDto.Image.FileName);
                var thumbnailsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads/Thumbnails");
                var imageFilePath = Path.Combine(thumbnailsFolder, imageFileName);

                if (!Directory.Exists(thumbnailsFolder))
                {
                    Directory.CreateDirectory(thumbnailsFolder);
                }

                // Save the Image file
                using (var stream = new FileStream(imageFilePath, FileMode.Create))
                {
                    await courseDto.Image.CopyToAsync(stream);
                }

                courseDto.Thumbnail = imageFileName;
            }

            
            var course = _mapper.Map<Course>(courseDto);
            course.CreatedAt = DateTime.Now;

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return _mapper.Map<CourseDTO>(course);
        }

        public async Task<bool> UpdateCourseAsync(int id, CourseDTO courseDto)
        {
            // Find the existing course
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return false;

            if (await IsOverlappingCourseAsync(courseDto, id))
            {
                throw new Exception("Cannot update course: Overlaps with an existing course.");
            }

            // Use AutoMapper to map the properties except Thumbnail
            _mapper.Map(courseDto, course);

            // Handle the thumbnail image
            if (courseDto.Image != null)
            {
                // Delete the old image if it exists
                if (!string.IsNullOrEmpty(course.Thumbnail))
                {
                    var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads/Thumbnails", course.Thumbnail);
                    if (File.Exists(oldImagePath))
                    {
                        File.Delete(oldImagePath);
                    }
                }

                // Save the new image
                var imageFileName = Guid.NewGuid() + Path.GetExtension(courseDto.Image.FileName);
                var thumbnailsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads/Thumbnails");
                var imageFilePath = Path.Combine(thumbnailsFolder, imageFileName);

                if (!Directory.Exists(thumbnailsFolder))
                {
                    Directory.CreateDirectory(thumbnailsFolder);
                }

                using (var stream = new FileStream(imageFilePath, FileMode.Create))
                {
                    await courseDto.Image.CopyToAsync(stream);
                }

                // Update the Thumbnail property in the course
                course.Thumbnail = imageFileName;
            }

            // Save changes with error handling
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Courses.Any(c => c.CourseId == id))
                    return false;

                throw; // Re-throw the exception for handling higher up if necessary
            }
            catch (Exception)
            {
                return false;
            }
        }
        private async Task<bool> IsOverlappingCourseAsync(CourseDTO courseDto, int? courseId)
        {
            var existingCourses = await _context.Courses
                .Where(c => c.CourseId != courseId) // Exclude the current course if updating
                .ToListAsync();

            // Get the days for the new course
            var newCourseDays = GetDays(courseDto);

            foreach (var existingCourse in existingCourses)
            {
                // Check if the existing course is active
                if (existingCourse.IsActive.GetValueOrDefault())
                {
                    // Check if the existing course has any of the same days as the new course
                    var existingCourseDays = GetDaysFromExisting(existingCourse);

                    // Check for overlap in days
                    bool daysOverlap = newCourseDays.Intersect(existingCourseDays).Any();

                    // Ensure StartDateAt and EndDateAt are not null
                    bool dateOverlap =
                        courseDto.StartDateAt.HasValue && existingCourse.StartDateAt.HasValue &&
                        courseDto.EndDateAt.HasValue && existingCourse.EndDateAt.HasValue &&
                        courseDto.StartDateAt <= existingCourse.EndDateAt &&
                        courseDto.EndDateAt >= existingCourse.StartDateAt;

                    // Ensure StartTimeAt and EndTimeAt are not null
                    bool timeOverlap =
                        courseDto.StartTimeAt.HasValue && existingCourse.StartTimeAt.HasValue &&
                        courseDto.EndTimeAt.HasValue && existingCourse.EndTimeAt.HasValue &&
                        courseDto.StartTimeAt < existingCourse.EndTimeAt &&
                        courseDto.EndTimeAt > existingCourse.StartTimeAt;

                    // Check for overlapping courses
                    if (daysOverlap && dateOverlap && timeOverlap)
                    {
                        return true; // Overlapping found
                    }
                }
            }

            return false; // No overlap
        }

        private List<DayOfWeek> GetDays(CourseDTO courseDto)
        {
            var days = new List<DayOfWeek>();

            if (courseDto.Monday.GetValueOrDefault()) days.Add(DayOfWeek.Monday);
            if (courseDto.Tuesday.GetValueOrDefault()) days.Add(DayOfWeek.Tuesday);
            if (courseDto.Wednesday.GetValueOrDefault()) days.Add(DayOfWeek.Wednesday);
            if (courseDto.Thursday.GetValueOrDefault()) days.Add(DayOfWeek.Thursday);
            if (courseDto.Friday.GetValueOrDefault()) days.Add(DayOfWeek.Friday);
            if (courseDto.Saturday.GetValueOrDefault()) days.Add(DayOfWeek.Saturday);
            if (courseDto.Sunday.GetValueOrDefault()) days.Add(DayOfWeek.Sunday);

            return days;
        }

        // New method to get days from the existing course model
        private List<DayOfWeek> GetDaysFromExisting(Course existingCourse)
        {
            var days = new List<DayOfWeek>();

            if (existingCourse.Monday.GetValueOrDefault()) days.Add(DayOfWeek.Monday);
            if (existingCourse.Tuesday.GetValueOrDefault()) days.Add(DayOfWeek.Tuesday);
            if (existingCourse.Wednesday.GetValueOrDefault()) days.Add(DayOfWeek.Wednesday);
            if (existingCourse.Thursday.GetValueOrDefault()) days.Add(DayOfWeek.Thursday);
            if (existingCourse.Friday.GetValueOrDefault()) days.Add(DayOfWeek.Friday);
            if (existingCourse.Saturday.GetValueOrDefault()) days.Add(DayOfWeek.Saturday);
            if (existingCourse.Sunday.GetValueOrDefault()) days.Add(DayOfWeek.Sunday);

            return days;
        }



        public async Task<bool> DeleteCourseAsync(int id)
        {
            // Fetch the course to ensure it exists along with associated data
            var course = await _context.Courses
                .Include(c => c.CartItems)
                .Include(c => c.OrderDetails)
                .Include(c => c.Registrations)
                .Include(c => c.Reviews)
                .Include(c => c.Topics)
                .ThenInclude(t => t.Lessons) // Include lessons associated with topics
                .Include(c => c.Categories) // Include categories associated with the course
                .FirstOrDefaultAsync(c => c.CourseId == id);

            if (course == null) return false;

            // Delete the associated image file if it exists
            if (!string.IsNullOrEmpty(course.Thumbnail))
            {
                var imageFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads/Thumbnails", course.Thumbnail);
                if (File.Exists(imageFilePath))
                {
                    File.Delete(imageFilePath);
                }
            }

            // Delete associated CartItems
            if (course.CartItems.Any())
            {
                _context.CartItems.RemoveRange(course.CartItems);
            }

            // Delete associated OrderDetails
            if (course.OrderDetails.Any())
            {
                _context.OrderDetails.RemoveRange(course.OrderDetails);
            }

            // Delete associated Registrations
            if (course.Registrations.Any())
            {
                _context.Registrations.RemoveRange(course.Registrations);
            }

            // Delete associated Reviews
            if (course.Reviews.Any())
            {
                _context.Reviews.RemoveRange(course.Reviews);
            }

            // Delete associated Topics and Lessons
            if (course.Topics.Any())
            {
                foreach (var topic in course.Topics)
                {
                    if (topic.Lessons.Any())
                    {
                        _context.Lessons.RemoveRange(topic.Lessons);
                    }
                }
                _context.Topics.RemoveRange(course.Topics);
            }

            // Remove associated Categories from the Course
            if (course.Categories.Any())
            {
                // This removes the associations from the course without deleting the actual categories
                course.Categories.Clear();
            }

            // Finally, remove the course
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<CourseDTO>> GetCoursesWithCategoriesAsync(IEnumerable<int> categoryIds)
        {
            var courses = await _context.Courses
                .Include(c => c.Categories)
                .Where(c => c.Categories.Any(cat => categoryIds.Contains(cat.CategoryId)))
                .ToListAsync();

            return _mapper.Map<IEnumerable<CourseDTO>>(courses);
        }

        public async Task<TimeOnly> GetCourseDurationAsync(int courseId)
        {
            // Fetch the course along with its topics and lessons
            var course = await _context.Courses
                .Include(c => c.Topics)
                    .ThenInclude(t => t.Lessons)
                .FirstOrDefaultAsync(c => c.CourseId == courseId);

            // Check if the course exists
            if (course == null)
            {
                return TimeOnly.MinValue; // Return a default value if the course doesn't exist
            }

            TimeSpan totalDuration = TimeSpan.Zero;

            // Iterate through each topic and each lesson to sum the durations
            foreach (var topic in course.Topics)
            {
                foreach (var lesson in topic.Lessons)
                {
                    if (lesson.Duration.HasValue)
                    {
                        totalDuration = totalDuration.Add(lesson.Duration.Value.ToTimeSpan());
                    }
                }
            }

            // Return total duration as TimeOnly
            return TimeOnly.FromTimeSpan(totalDuration);
        }


    }
}
