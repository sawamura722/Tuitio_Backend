namespace Tuitio.DTOs
{
    public class CourseDTO
    {
        public int? CourseId { get; set; }

        public string CourseTitle { get; set; } = null!;

        public string? CourseDescription { get; set; }

        public decimal Price { get; set; }

        public bool IsOnline { get; set; }

        public int TeacherId { get; set; }

        public string? Thumbnail { get; set; }
        public IFormFile? Image { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedAt { get; set; }
        public string? Location { get; set; }
        public int? Limit { get; set; }

        public TimeOnly? StartTimeAt { get; set; }

        public TimeOnly? EndTimeAt { get; set; }

        public DateOnly? StartDateAt { get; set; }

        public DateOnly? EndDateAt { get; set; }
        public bool? Monday { get; set; }

        public bool? Tuesday { get; set; }

        public bool? Wednesday { get; set; }

        public bool? Thursday { get; set; }

        public bool? Friday { get; set; }

        public bool? Saturday { get; set; }

        public bool? Sunday { get; set; }
    }
}
