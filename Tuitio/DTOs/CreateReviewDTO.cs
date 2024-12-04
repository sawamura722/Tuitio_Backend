namespace Tuitio.DTOs
{
    public class CreateReviewDTO
    {
        public int? ReviewId { get; set; }

        public int StudentId { get; set; }

        public int CourseId { get; set; }

        public int? Rating { get; set; }

        public string? Comment { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
