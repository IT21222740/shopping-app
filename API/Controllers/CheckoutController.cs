using Application.DTOs;
using Application.DTOs.Order;
using Application.DTOs.Product;
using Application.Interfaces;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Diagnostics;

namespace API.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ICheckoutService _checkoutService;

        public CheckoutController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        [Authorize]
        [HttpPost("Checkout")]
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
        [HttpPost("webHook")]
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
        [Authorize]
        [HttpGet("Get-user-payments")]
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

        [Authorize]
        [HttpGet("Get-User-payments-byId")]
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
