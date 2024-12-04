using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Tuitio.Services.IService;
using Tuitio.Models;
using Microsoft.AspNetCore.Authorization;

namespace Tuitio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [Authorize(Roles = "USER")]
        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetCart(int studentId)
        {
            var cart = await _cartService.GetCartByStudentIdAsync(studentId);
            
            return Ok(cart);
        }

        [Authorize(Roles = "USER")]
        [HttpPost("{studentId}/add/{courseId}")]
        public async Task<IActionResult> AddItemToCart(int studentId, int courseId)
        {
            await _cartService.AddItemToCartAsync(studentId, courseId);
            return Ok("Course added to cart");
        }

        [Authorize(Roles = "USER")]
        [HttpDelete("{studentId}/remove/{courseId}")]
        public async Task<IActionResult> RemoveItemFromCart(int studentId, int courseId)
        {
            await _cartService.RemoveItemFromCartAsync(studentId, courseId);
            return Ok("Course removed from cart");
        }

        [Authorize(Roles = "USER")]
        [HttpDelete("{studentId}/clear")]
        public async Task<IActionResult> ClearCart(int studentId)
        {
            await _cartService.ClearCartAsync(studentId);
            return Ok("Cart cleared");
        }

        [Authorize(Roles = "USER")]
        [HttpPost("{studentId}/checkout")]
        public async Task<IActionResult> Checkout(int studentId, string paymentMethodId, string returnUrl, string receiptEmail)
        {
            try
            {
                var cart = await _cartService.CheckoutCartAsync(studentId, paymentMethodId, returnUrl, receiptEmail);
                return Ok("Cart checked out successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
