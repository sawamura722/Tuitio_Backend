using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tuitio.DTOs;
using Tuitio.Services.IService;

namespace Tuitio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize(Roles = "ADMIN,TEACHER")]
        [HttpGet]
        public async Task<ActionResult<List<OrderDTO>>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [Authorize(Roles = "USER")]
        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<List<OrderDTO>>> GetOrdersByStudentId(int studentId)
        {
            var orders = await _orderService.GetOrdersByStudentIdAsync(studentId);
            return Ok(orders);
        }

        [Authorize(Roles = "USER")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await _orderService.DeleteOrderAsync(id);
            if (!result)
            {
                return NotFound(); // Return 404 if order not found
            }

            return NoContent(); // Return 204 No Content on successful deletion
        }
    }
}
