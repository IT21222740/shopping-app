using Application.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface ICheckoutService
    {
        Task<string> checkout();
        Task<ServiceResponse> HandlePayment();
        Task<ServiceResponse> GetAllUserPayments();
        Task<ServiceResponse> GetPaymentInfoById(int orderId);


    }
}
