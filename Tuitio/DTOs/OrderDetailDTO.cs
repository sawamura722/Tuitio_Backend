namespace Tuitio.DTOs
{
    public class OrderDetailDTO
    {
        public int? OrderDetailId { get; set; }
        public int CourseId { get; set; }
        public string CourseTitle { get; set; } 
        public decimal Price { get; set; }
    }
}
