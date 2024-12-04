using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tuitio.DTOs;
using Tuitio.Models;
using Tuitio.Services.IService;

namespace Tuitio.Services.ServiceImpl
{
    public class OrderService : IOrderService
    {
        private readonly TutoringSchoolContext _context;
        private readonly IMapper _mapper;

        public OrderService(TutoringSchoolContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Get all orders
        public async Task<List<OrderDTO>> GetAllOrdersAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Course) // Include course details within order details
                .ToListAsync();

            return _mapper.Map<List<OrderDTO>>(orders);
        }

        // Get orders by student ID
        public async Task<List<OrderDTO>> GetOrdersByStudentIdAsync(int studentId)
        {
            var orders = await _context.Orders
                .Where(o => o.StudentId == studentId)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Course) // Include course details within order details
                .ToListAsync();

            return _mapper.Map<List<OrderDTO>>(orders);
        }

        // Delete an order by ID
        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails) // Include related OrderDetails
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return false; // Order not found
            }

            // Remove associated OrderDetails first
            _context.OrderDetails.RemoveRange(order.OrderDetails);

            // Now remove the order
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync(); // Save changes to database

            return true; // Order deleted successfully
        }
    }
}
