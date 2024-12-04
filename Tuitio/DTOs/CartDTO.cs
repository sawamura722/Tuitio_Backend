namespace Tuitio.DTOs
{
    public class CartDTO
    {
        public int? CartId { get; set; }

        public int StudentId { get; set; }

        // Add a collection of CartItemDTO to hold the items in the cart
        public List<CartItemDTO> CartItems { get; set; } = new List<CartItemDTO>();

    }
}
