using Application.DTOs.Cart;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Net.Mime;

namespace API.Controllers
{
 

    [Route("api/[Controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiController]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        /// <summary>
        /// Add a item to the cart
        /// </summary>
        /// <param name="cartItem"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("add-to-cart")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddToCart([FromBody] CartItemRequset cartItem)
        {
            var result = await _cartService.AddCartItem(cartItem);
            if(result.Flag == true)
            {
                return Ok(result.Message);
            }else return BadRequest(result.Message);
        }

        /// <summary>
        /// Update items by sending productId and amount
        /// </summary>
        /// <param name="cartItem"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("modify-cart-item")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ModifyCart([FromBody] CartItemRequset cartItem)
        {
            var result = await _cartService.ModifyCartItem(cartItem);
            if (result.Flag == true)
            {
                return Ok(result.Message);
            }
            else return BadRequest(result.Message);
        }

        /// <summary>
        /// delete Items from the cart
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("delete-cart-item/{productId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteCartItem(int productId)
        {
            var result = await _cartService.RemoveCartItem(productId);
            if(result.Flag == true)
            {
                return Ok(result.Message);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        /// <summary>
        /// View Items in the cart
        /// View Items in the cart
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("get-all-cartItems")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
