namespace Tuitio.DTOs
{
    public class OrderDTO
    {
        public int? OrderId { get; set; }

        public int StudentId { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime? CreatedAt { get; set; }
        public List<OrderDetailDTO> OrderDetails { get; set; } = new List<OrderDetailDTO>();
    }
}
