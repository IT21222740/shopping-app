using Application.DTOs;
using Application.DTOs.Order;
using Application.DTOs.Product;
using Application.Interfaces;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Diagnostics;
using System.Net.Mime;

namespace API.Controllers
{
    [Route("api/[Controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiController]
    public class PaymentController : Controller
    {
        private readonly ICheckoutService _checkoutService;

        public PaymentController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }
        /// <summary>
        /// user can perform payment
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Checkout()
        {
            var response = await _checkoutService.checkout();
            if (response.Equals("error"))
            {
                return BadRequest("Stock currently Unavailable");
            }
            Response.Headers.Add("Location", response);
            Process.Start(new ProcessStartInfo("cmd", $"/c start {response}")
            {
                CreateNoWindow = true
            });
            return Ok(response);

        }
        /// <summary>
        /// This is a webhook used by stripe service
        /// </summary>
        /// <returns></returns>
        [HttpPost("webHook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Index()
        {
            ServiceResponse response = await _checkoutService.HandlePayment();

            if(response.Flag == true)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest();
            }
            
        }
        /// <summary>
        /// Retrieving user payments
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("Get-user-payments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UserPayments()
        {
            ServiceResponse response = await _checkoutService.GetAllUserPayments();
            if(response.Flag == true)
            {
                return Ok(response.data);
            }
            else
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// Get User payement for a specific order
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("Get-User-payments-byId/{orderId:int}")]
        public async Task<IActionResult> GetPaymentInfo(int orderId)
        {
            ServiceResponse response = await _checkoutService.GetPaymentInfoById(orderId);
            if(response.Flag == true)
            {
                return Ok(response.data);
            }
            else
            {
                return BadRequest();
            }
        }

    }
}
