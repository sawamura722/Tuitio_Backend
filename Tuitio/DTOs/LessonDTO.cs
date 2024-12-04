namespace Tuitio.DTOs
{
    public class LessonDTO
    {
        public int? LessonId { get; set; }

        public string LessonTitle { get; set; } = null!;

        public string? LessonContent { get; set; }

        public string? VideoUrl { get; set; }
        public IFormFile? Video { get; set; }

        public string? Thumbnail { get; set; }
        public int? TopicId { get; set; }
        public IFormFile? Image { get; set; }
        public TimeOnly? Duration { get; set; }

    }
}
