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
        private readonly ICheckoutService _chckoutService;

        public CheckoutController(ICheckoutService checkoutService)
        {
            _chckoutService = checkoutService;
        }

        [Authorize]
        [HttpPost("Checkout")]
        public async Task<IActionResult> Checkout()
        {
            var url = await _chckoutService.checkout();
            Response.Headers.Add("Location", url);
            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}")
            {
                CreateNoWindow = true
            });
            return Ok(url);

        }
        [HttpPost("webHook")]
        public async Task<IActionResult> Index()
        {
            ServiceResponse response = await _chckoutService.HandlePayment();

            if(response.Flag == true)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest();
            }
          

            
        }

    }
}
