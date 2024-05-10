using Application.Interfaces;
using Domain.Entities;
using SendGrid;
using SendGrid.Helpers.Mail;
using Stripe.Climate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Order = Domain.Entities.Order;

namespace Infrastructure.EmailService
{
    public class EmailSender : IEmailSender
    {
        public async Task Execute(Order order)
        {
            var client = new SendGridClient("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");
            var from = new EmailAddress("tharushathejanofficial@gmail.com","Tharusha Thejan");
            var subject = "Order Invoice";
            var to = new EmailAddress(order?.User?.Email, order?.User?.FirstName);
            var palinText = "Here's you Invoice";
            var htmlContent =genterateHtml(order);
            
            var msg = MailHelper.CreateSingleEmail(from,to,subject, palinText, htmlContent);
            var response = await client.SendEmailAsync(msg);
            
        }

        public static string genterateHtml(Order order)
        {
            StringBuilder invoiceContent = new StringBuilder();

            // Start HTML document
            invoiceContent.AppendLine("<!DOCTYPE html>");
            invoiceContent.AppendLine("<html lang=\"en\">");
            invoiceContent.AppendLine("<head>");
            invoiceContent.AppendLine("<meta charset=\"UTF-8\">");
            invoiceContent.AppendLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
            invoiceContent.AppendLine("<title>Invoice</title>");

            // CSS styles
            invoiceContent.AppendLine("<style>");
            invoiceContent.AppendLine("body { font-family: Arial, sans-serif; background-color: #FEFAF6;}");
            invoiceContent.AppendLine("h1, h2 { text-align: center; }");
            invoiceContent.AppendLine("table { width: 100%; border-collapse: collapse; }");
            invoiceContent.AppendLine("th, td { padding: 8px; text-align: left; border-bottom: 1px solid #ddd; }");
            invoiceContent.AppendLine("th { background-color: #f2f2f2; }");
            invoiceContent.AppendLine("</style>");

            invoiceContent.AppendLine("</head>");
            invoiceContent.AppendLine("<body>");

            // Invoice header
            invoiceContent.AppendLine("<h1>Invoice</h1>");
            invoiceContent.AppendLine("<hr>");

            // Customer information
            invoiceContent.AppendLine("<h2>Customer Information</h2>");
            invoiceContent.AppendLine($"<p><strong>Name:</strong> {order.User?.FirstName}</p>");
            //invoiceContent.AppendLine($"<p><strong>Address:</strong> {order.User.}</p>");
            invoiceContent.AppendLine($"<p><strong>Email:</strong> {order.User?.Email}</p>");
            invoiceContent.AppendLine("<hr>");

            // Order details
            invoiceContent.AppendLine("<h2>Order Details</h2>");
            invoiceContent.AppendLine("<table>");
            invoiceContent.AppendLine("<tr><th>Product</th><th>Price</th><th>Quantity</th><th>Total</th></tr>");
            

            if(order.OrderProduct is not null)
            {
                foreach (var item in order.OrderProduct)
                {
                    invoiceContent.AppendLine($"<tr><td>{item.Product?.Name}</td><td>${item.UnitPrice.ToString("N0")}</td><td>{item.OrderQty}</td><td>${item.UnitPrice * item.OrderQty}</td></tr>");
                    
                }
            }
            
            invoiceContent.AppendLine("</table>");
            invoiceContent.AppendLine("<hr>");

            // Total amount
            invoiceContent.AppendLine($"<h2>Total Amount: ${order.TotalAmount.ToString("N0")}</h2>");

            // End HTML document
            invoiceContent.AppendLine("</body>");
            invoiceContent.AppendLine("</html>");

            return invoiceContent.ToString();
        }
    }
    
}
