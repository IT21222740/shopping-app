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

namespace Application.Services.Implementation
{

    public class CheckoutService : ICheckoutService
    {
        private readonly ITokenService _tokenService;
        private readonly IPamentService _pamentService;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserProduct> _userProducts;
        private readonly IOrderService _orderService;
        private readonly IEmailSender _emailSender;
        public CheckoutService(ITokenService tokenService, IPamentService pamentService,IRepository<User> userRepository,IRepository<UserProduct> userProdcuts, IOrderService orderService,IEmailSender emailSender) {
            _tokenService = tokenService;
            _pamentService = pamentService;
            _userRepository = userRepository;
            _userProducts = userProdcuts;
            _orderService = orderService;
            _emailSender = emailSender;
        }
        public async Task<string> checkout()
        {
            var userId = _tokenService.GetUserId();
            var User = await _userRepository.Get(u=>u.UserId == userId);
            var results = await _userProducts.GetAll(filter: up => up.userId == userId, includePropeties: "Product");
            var result = await _orderService.AddOrder(results);
            var url =  await _pamentService.Checkout(User.StripeId,results.ToList(), result.data.ToString());
            return url;
          
        }

        public async Task<ServiceResponse> HandlePayment()
        {
            var response = await _pamentService.webHookHandler();
            if(response.Exception is not null)
            {
                return new ServiceResponse(false, response.Exception.Message, response.Exception);
            }else if(response.PaymentStatus == true && response.CheckoutResponse is not null)
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
    }

}

