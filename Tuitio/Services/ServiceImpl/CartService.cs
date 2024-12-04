using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Tuitio.DTOs;
using Tuitio.Models;
using Tuitio.Services.IService;
using Stripe;

namespace Tuitio.Services.ServiceImpl
{
    public class CartService : ICartService
    {
        private readonly TutoringSchoolContext _context;
        private readonly IMapper _mapper;
        private readonly PaymentService _paymentService;

        public CartService(TutoringSchoolContext context, IMapper mapper, PaymentService paymentService)
        {
            _context = context;
            _mapper = mapper;
            _paymentService = paymentService;
        }

        // Get cart by student ID
        public async Task<CartDTO> GetCartByStudentIdAsync(int studentId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Course)
                .FirstOrDefaultAsync(c => c.StudentId == studentId);

            return _mapper.Map<CartDTO>(cart);
        }

        // Add item to cart
        public async Task AddItemToCartAsync(int studentId, int courseId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.StudentId == studentId);

            if (cart == null)
            {
                cart = new Cart { StudentId = studentId };
                _context.Carts.Add(cart);
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.CourseId == courseId);

            if (cartItem == null)
            {
                cart.CartItems.Add(new CartItem { CourseId = courseId });
            }

            await _context.SaveChangesAsync();
        }

        // Remove an item from cart
        public async Task<bool> RemoveItemFromCartAsync(int studentId, int courseId)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.Cart.StudentId == studentId && ci.CourseId == courseId);

            if (cartItem == null)
                return false;

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();  // Ensure changes are saved after deletion

            return true;
        }

        // Clear the cart
        public async Task ClearCartAsync(int studentId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.StudentId == studentId);

            if (cart != null)
            {
                cart.CartItems.Clear();
                await _context.SaveChangesAsync();
            }
        }

        // Checkout the cart
        // Checkout the cart
        public async Task<CartDTO> CheckoutCartAsync(int studentId, string paymentMethodId, string returnUrl, string receiptEmail)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Course)
                .FirstOrDefaultAsync(c => c.StudentId == studentId);

            if (cart == null || !cart.CartItems.Any())
            {
                throw new Exception("Cart is empty.");
            }

            var totalPrice = cart.CartItems.Sum(ci => ci.Course.Price);

            // Create a payment using the injected PaymentService
            var paymentResult = await _paymentService.ProcessPaymentAsync(paymentMethodId, totalPrice, returnUrl, receiptEmail);

            if (!paymentResult.IsSuccess)
            {
                throw new Exception("Payment failed: " + paymentResult.ErrorMessage);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Create an order
                var order = new Order
                {
                    StudentId = studentId,
                    TotalPrice = totalPrice,
                    CreatedAt = DateTime.Now
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync(); // Save changes to get the order ID

                // Create OrderDetails and register the student for each course in the cart
                foreach (var cartItem in cart.CartItems)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = order.OrderId,
                        CourseId = cartItem.CourseId,
                        Price = cartItem.Course.Price
                    };

                    _context.OrderDetails.Add(orderDetail);

                    var existingRegistration = await _context.Registrations
                        .FirstOrDefaultAsync(r => r.StudentId == studentId && r.CourseId == cartItem.CourseId);

                    if (existingRegistration == null)
                    {
                        var registration = new Registration
                        {
                            StudentId = studentId,
                            CourseId = cartItem.CourseId,
                            RegistrationDate = DateTime.Now
                        };

                        _context.Registrations.Add(registration);
                    }
                }

                // Remove all cart items and the cart itself
                _context.CartItems.RemoveRange(cart.CartItems);
                _context.Carts.Remove(cart);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

            return _mapper.Map<CartDTO>(cart);
        }

    }
}
