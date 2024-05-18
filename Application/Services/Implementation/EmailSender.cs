using Application.DTOs.Email;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Services.Helpers;
using Application.Services.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Order = Domain.Entities.Order;
using Microsoft.Extensions.Configuration;

namespace Application.EmailService
{
    public class EmailSenderService : IEmailSenderService
    {

        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<IEmailService> _logger;

        public EmailSenderService(IConfiguration configuration, IEmailService emailService, ILogger<IEmailService> logger)
        {
            _configuration = configuration;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task Execute(Order order)
        {
            try
            {
                var orderProducts = order?.OrderProduct?.Select(p => new
                {
                    productName = p.Product?.Name,
                    quantity = p.OrderQty,
                    unitPrice = p.UnitPrice.ToString("N0"),
                    totalPrice = (p.OrderQty * p.UnitPrice).ToString("N0")
                }).ToList();


                var templateData = new Dictionary<string, object>
                    {
                        { "recipientFirstName", order?.User?.FirstName ?? "" },
                        { "orders", orderProducts },
                        { "payment_id", order.PaymentId },
                        { "Order_Id", order.OrderId },
                        { "Order_date", order.OrderDate },
                        { "totalAmount", order.OrderProduct.Sum(p => p.OrderQty * p.UnitPrice).ToString("N0") },
                        { "orderStatus", "Completed" },
                        { "current_year", DateTime.Now.Year },
                    };


                SendEmailModelDTO sendEmailModelDTO = new()
                {
                    ReceiverEmails = new List<string> { "order?.User?.Email" },
                    SenderEmail = _configuration["EmailServiceSettings:SenderEmail"] ?? "info@oxygen.com.au",
                    SenderName = _configuration["EmailServiceSettings:SenderName"] ?? "Oxygen",
                    Subject = "Here's you Invoice",
                    TemplateId = _configuration["EmailServiceSettings:Templates:PaymentSuccessfulTemplateId"],
                    TemplateData = templateData
                };

                await _emailService.SendEmailAsync(sendEmailModelDTO);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "{Message}", CompleteExceptionMessage.Get(ex));
            }
        }

        public class TempOrder
        {
            public IEnumerable<OrderProduct> OrderProducts { get; set; }
            // Additional properties can be included as needed
        }

        public class TempOrderProduct
        {
            public required TempProduct Product { get; set; }
            public int Order_Qty { get; set; }
            public decimal Unit_Price { get; set; }
        }

        public class TempProduct
        {
            public required string Name { get; set; }
        }
    }

}
