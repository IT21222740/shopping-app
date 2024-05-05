using Application.DTOs.Cart;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        [Authorize]
        [HttpPost("add-to-cart")]
        public async Task<IActionResult> AddToCart([FromBody] CartItemRequset cartItem)
        {
            var result = await _cartService.AddCartItem(cartItem);
            if(result.Flag == true)
            {
                return Ok(result.Message);
            }else return BadRequest(result.Message);
        }

        [Authorize]
        [HttpPut("modify-cart-item")]
       public async Task<IActionResult> ModifyCart([FromBody] CartItemRequset cartItem)
        {
            var result = await _cartService.ModifyCartItem(cartItem);
            if (result.Flag == true)
            {
                return Ok(result.Message);
            }
            else return BadRequest(result.Message);
        }
        [Authorize]
        [HttpDelete("delete-cart-item")]
        public async Task<IActionResult> DeleteCartItem([FromBody] CartItemRequset cartItem)
        {
            var result = await _cartService.RemoveCartItem(cartItem);
            if(result.Flag == true)
            {
                return Ok(result.Message);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        [Authorize]
        [HttpGet("getAll-cartItems")]
        public async Task<IActionResult> GetAllCartItems()
        {
            var result = await _cartService.ViewCart();
            if (result.Flag == true)
            {
                return Ok(result.data);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }
    }
}
