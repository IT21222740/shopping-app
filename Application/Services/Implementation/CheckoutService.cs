using Microsoft.Extensions.Logging;
using Application.Services.Interfaces;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Application.DTOs;
using Application.DTOs.Order;
using System.Reflection.Metadata.Ecma335;


namespace Application.Services.Implementation
{

    public class CheckoutService : ICheckoutService
    {
        private readonly ITokenService _tokenService;
        private readonly IPamentService _pamentService;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserProduct> _userProducts;
        private readonly IOrderService _orderService;
        private readonly IEmailSenderService _emailSender;
        private readonly IRepository<OrderProduct> _orderProducts;
        private readonly ICartService _cartService;
        public CheckoutService(ITokenService tokenService, IPamentService pamentService, IRepository<User> userRepository, IRepository<UserProduct> userProdcuts, IOrderService orderService, IEmailSenderService emailSender, IRepository<OrderProduct> orderProducts, ICartService cartService)
        {
            _tokenService = tokenService;
            _pamentService = pamentService;
            _userRepository = userRepository;
            _userProducts = userProdcuts;
            _orderService = orderService;
            _emailSender = emailSender;
            _orderProducts = orderProducts;
            _cartService = cartService;
            

        }
        public async Task<string> checkout()
        {
            var userId = _tokenService.GetUserId();
            var User = await _userRepository.Get(u=>u.UserId == userId);
            var results = await _userProducts.GetAll(filter: up => up.userId == userId, includePropeties: "Product");
            var result = await _cartService.checkAvailabilty();

            if (result)
            {
                 var response = await _orderService.AddOrder(results);
                 var url =  await _pamentService.Checkout(User.StripeId,results.ToList(), response.data.ToString());
                 return url;
            }
            else
            {
                return "error";
            }
           
          
        }

        public async Task<ServiceResponse> HandlePayment()
        {
            var response = await _pamentService.webHookHandler();
            if(response.Exception is not null)
            {
                return new ServiceResponse(false, response.Exception.Message, response.Exception);
            }else if(response.CheckoutResponse is not null)
            {
                await _orderService.UpdateOrder(response.CheckoutResponse);
                Order order = await _orderService.GetCurrentOrderInfo(response.CheckoutResponse.OrderId);
                await _emailSender.Execute(order);

                return new ServiceResponse(true, "Complete Payment");
            }
            else
            {
                return new ServiceResponse(Flag:true, Message: "Unhandled event");
            }

        }

        public async Task<ServiceResponse> GetAllUserPayments()
        {
            var userId = _tokenService.GetUserId();
            if(userId != null)
            {
                var paymentList = await _orderService.GetUserOrders(userId);
                return new ServiceResponse(true,"Success", paymentList);
                
            }
            else
            {
                return new ServiceResponse(false, "Userid not found");
            }
            
            
        }

        public async Task<ServiceResponse> GetPaymentInfoById(int orderId)
        {
            var orderProducts = await _orderProducts.GetAll(filter: op => op.OrderId == orderId, includePropeties: "Product");

            IEnumerable<PaymentProductDto> list = orderProducts.Select(op => new PaymentProductDto
            {
                Price = op.UnitPrice,
                Quantity = op.OrderQty,
                ImageUrl = op.Product.ImageUrl,
                Name = op.Product.Name,
                PriceTotal = (decimal)(op.OrderQty * op.UnitPrice),


            });
            
            return new ServiceResponse(true, "Success", list.ToList());
        }
    }

}

