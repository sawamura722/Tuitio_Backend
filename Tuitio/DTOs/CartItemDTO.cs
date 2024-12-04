namespace Tuitio.DTOs
{
    public class CartItemDTO
    {
        public int CartItemId { get; set; }
        public int CourseId { get; set; }
        public string CourseTitle { get; set; } 
        public decimal Price { get; set; } 
        public string Thumbnail { get; set; } 
    }
}
