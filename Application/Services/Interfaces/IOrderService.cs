using Application.DTOs;
using Application.DTOs.Order;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IOrderService
    {
        Task<ServiceResponse> AddOrder(IEnumerable<UserProduct >userProducts);
        Task<ServiceResponse> UpdateOrder(PaymentCreation paymentResponse);
        Task<Order> GetCurrentOrderInfo(int orderId);
    }
}
