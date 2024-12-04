using Tuitio.DTOs;
using Tuitio.Models;

namespace Tuitio.Services.IService
{
    public interface ICartService
    {
        Task<CartDTO> GetCartByStudentIdAsync(int studentId);
        Task AddItemToCartAsync(int studentId, int courseId);
        Task<bool> RemoveItemFromCartAsync(int studentId, int courseId);
        Task ClearCartAsync(int studentId);
        Task<CartDTO> CheckoutCartAsync(int studentId, string paymentMethodId, string returnUrl, string receiptEmail);
    }

}
