using Application.DTOs.Order;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPamentService
    {
        Task<string> RegisterUserToPayment(string email, string name);
        Task<string> Checkout(string stripeId,IEnumerable<UserProduct> productList, string orderId);
        Task<WebHookResponse> webHookHandler();

        //string GetPaymentInfo();
    }
}
