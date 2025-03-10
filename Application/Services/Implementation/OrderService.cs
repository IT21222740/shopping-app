﻿using Application.DTOs;
using Application.DTOs.Cart;
using Application.DTOs.Order;
using Application.Interfaces.Repositories;
using Application.Services.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<OrderProduct> _orderProductRepository;
        private readonly ITokenService _tokenService;
        private readonly ICartService _cartService;
        private readonly IRepository<Product> _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        public OrderService(IRepository<Order> orderRepository, IRepository<OrderProduct> productRepository, ITokenService tokenService, ICartService cartService, IRepository<Product> productsRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _orderProductRepository = productRepository;
            _tokenService = tokenService;
            _cartService = cartService;
            _productRepository = productsRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ServiceResponse> AddOrder(IEnumerable<UserProduct> userProducts)
        {
            var userId = _tokenService.GetUserId();
            DateTime today = DateTime.Now;

            Order order = new Order {
                UserId = userId,
                OrderDate = today,
                Status = "processing"

            };
            await _orderRepository.Add(order);
            if (order is not null)
            {
                foreach (var product in userProducts)
                {
                    OrderProduct orderProduct = new OrderProduct
                    {
                        ProductId = product.ProductId,
                        OrderId = order.OrderId,
                        UnitPrice = product.Product.Price,
                        OrderQty = product.Quantity

                    };
                    await _orderProductRepository.Add(orderProduct);
                }

            }
            return new ServiceResponse(true, "added", order.OrderId);

        }


        public async Task<ServiceResponse> UpdateOrder(PaymentCreation paymentResponse)
        {
            var order = await _orderRepository.Get(filter: o => o.OrderId == paymentResponse.OrderId);

            if (order is not null)
            {
                
                order.PaymentId = paymentResponse.PaymentId;
                order.Status = paymentResponse.Status;
                order.TotalAmount = paymentResponse.TotalAmount;

                await _unitOfWork.BeginTransactionAsync();
                try
                {
                    await _orderRepository.Update(order);

                    var response = await _cartService.GetItems(order.UserId);

                    foreach (var cartItem in response)
                    {
                        var product = await _productRepository.Get(filter: p => p.ProductId == cartItem.ProductId);
                        if (product is not null && product.Quantity >= cartItem.Quantity)
                        {
                            product.Quantity = product.Quantity - cartItem.Quantity;
                            await _productRepository.Update(product);
                        }
                    }

                    await _cartService.ClearCart(order.UserId);
                    

                    await _unitOfWork.CommitAsync();

                    return new ServiceResponse(true, "Updated Order successfully", order);
                }
                catch(Exception ex)
                {
                    await _unitOfWork.RollbackAsync();
                    return new ServiceResponse(false, "Updated Order Unsucess", ex);
                }

               
                
                

                

            }

            return new ServiceResponse(false, "Order Id is incorrect");
        }

        public async Task<Order> GetCurrentOrderInfo(int orderId)
        {
            var order = await _orderRepository.Get(filter: o => o.OrderId == orderId, includePropeties: "User,OrderProduct,OrderProduct.Product");
            return order;

        }

        public async Task<IEnumerable<PaymentInfoDTO>> GetUserOrders(string userId)
        {
            var orders = await _orderRepository.GetAll(filter: o => o.UserId == userId);
            var result = orders.Select(o => new PaymentInfoDTO
            {
                OrderDate = o.OrderDate,
                OrderId = o.OrderId,
                PaymentId = o.PaymentId,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
            }).ToList();


            return result;
        }
    }
}
