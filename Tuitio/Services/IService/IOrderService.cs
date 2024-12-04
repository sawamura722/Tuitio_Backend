using Tuitio.DTOs;

namespace Tuitio.Services.IService
{
    public interface IOrderService
    {
        Task<List<OrderDTO>> GetAllOrdersAsync();
        Task<List<OrderDTO>> GetOrdersByStudentIdAsync(int studentId);
        Task<bool> DeleteOrderAsync(int orderId);
    }
}
