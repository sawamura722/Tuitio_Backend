using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NReco.VideoInfo;
using System;
using Tuitio.DTOs;
using Tuitio.Models;
using Tuitio.Services.IService;
using NReco.VideoInfo;

public class LessonService : ILessonService
{
    private readonly TutoringSchoolContext _context;
    private readonly IMapper _mapper;

    public LessonService(TutoringSchoolContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // Fetch lessons by CourseId
    public async Task<IEnumerable<LessonDTO>> GetLessonsByTopicIdAsync(int topicId)
    {
        var lessons = await _context.Lessons
            .Where(l => l.TopicId == topicId)
            .ToListAsync();

        return _mapper.Map<IEnumerable<LessonDTO>>(lessons);
    }

    public async Task<LessonDTO> GetLessonByIdAsync(int lessonId)
    {
        var lesson = await _context.Lessons.FindAsync(lessonId);
        if (lesson == null) return null;

        return _mapper.Map<LessonDTO>(lesson);
    }

    public async Task<LessonDTO> CreateLessonAsync(LessonDTO lessonDto)
    {
        if (lessonDto.Image != null)
        {
            var imageFileName = Guid.NewGuid() + Path.GetExtension(lessonDto.Image.FileName);
            var thumbnailsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads/Thumbnails");
            var imageFilePath = Path.Combine(thumbnailsFolder, imageFileName);

            using (var stream = new FileStream(imageFilePath, FileMode.Create))
            {
                await lessonDto.Image.CopyToAsync(stream);
            }

            lessonDto.Thumbnail = imageFileName;
        }

        // Handle Video upload
        if (lessonDto.Video != null)
        {
            var videoFileName = Guid.NewGuid() + Path.GetExtension(lessonDto.Video.FileName);
            var videosFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads/Videos");
            var videoFilePath = Path.Combine(videosFolder, videoFileName);

            using (var stream = new FileStream(videoFilePath, FileMode.Create))
            {
                await lessonDto.Video.CopyToAsync(stream);
            }

            lessonDto.VideoUrl = videoFileName;

            // Extract video duration
            var ffProbe = new FFProbe();
            var videoInfo = ffProbe.GetMediaInfo(videoFilePath);
            lessonDto.Duration = TimeOnly.FromTimeSpan(videoInfo.Duration);
        }

        var lesson = _mapper.Map<Lesson>(lessonDto);

        _context.Lessons.Add(lesson);
        await _context.SaveChangesAsync();

        return _mapper.Map<LessonDTO>(lesson);
    }

    public async Task<bool> UpdateLessonAsync(int lessonId, LessonDTO lessonDto)
    {
        var lesson = await _context.Lessons.FindAsync(lessonId);
        if (lesson == null) return false;

        // Delete existing image if a new one is uploaded
        if (lessonDto.Image != null && !string.IsNullOrEmpty(lesson.Thumbnail))
        {
            var existingImagePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads/Thumbnails", lesson.Thumbnail);
            if (File.Exists(existingImagePath))
            {
                File.Delete(existingImagePath);
            }

            var imageFileName = Guid.NewGuid() + Path.GetExtension(lessonDto.Image.FileName);
            var thumbnailsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads/Thumbnails");
            var imageFilePath = Path.Combine(thumbnailsFolder, imageFileName);

            using (var stream = new FileStream(imageFilePath, FileMode.Create))
            {
                await lessonDto.Image.CopyToAsync(stream);
            }

            lesson.Thumbnail = imageFileName;
        }

        // Delete existing video if a new one is uploaded
        if (lessonDto.Video != null && !string.IsNullOrEmpty(lesson.VideoUrl))
        {
            var existingVideoPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads/Videos", lesson.VideoUrl);
            if (File.Exists(existingVideoPath))
            {
                File.Delete(existingVideoPath);
            }

            var videoFileName = Guid.NewGuid() + Path.GetExtension(lessonDto.Video.FileName);
            var videosFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads/Videos");
            var videoFilePath = Path.Combine(videosFolder, videoFileName);

            using (var stream = new FileStream(videoFilePath, FileMode.Create))
            {
                await lessonDto.Video.CopyToAsync(stream);
            }

            lesson.VideoUrl = videoFileName;

            // Extract video duration
            var ffProbe = new FFProbe();
            var videoInfo = ffProbe.GetMediaInfo(videoFilePath);
            lesson.Duration = TimeOnly.FromTimeSpan(videoInfo.Duration);

        }

        lesson.LessonTitle = lessonDto.LessonTitle;
        lesson.LessonContent = lessonDto.LessonContent;

        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Lessons.Any(l => l.LessonId == lessonId))
                return false;
            throw;
        }
    }


    public async Task<bool> DeleteLessonAsync(int lessonId)
    {
        var lesson = await _context.Lessons.FindAsync(lessonId);
        if (lesson == null) return false;

        // Delete the associated image file
        if (!string.IsNullOrEmpty(lesson.Thumbnail))
        {
            var imageFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads/Thumbnails", lesson.Thumbnail);
            if (File.Exists(imageFilePath))
            {
                File.Delete(imageFilePath);
            }
        }

        // Delete the associated video file
        if (!string.IsNullOrEmpty(lesson.VideoUrl))
        {
            var videoFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads/Videos", lesson.VideoUrl);
            if (File.Exists(videoFilePath))
            {
                File.Delete(videoFilePath);
            }
        }

        _context.Lessons.Remove(lesson);
        await _context.SaveChangesAsync();
        return true;
    }

}