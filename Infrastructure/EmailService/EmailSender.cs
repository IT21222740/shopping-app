using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Diagnostics;
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
            var from = new EmailAddress("tharushathejanofficial@gmail.com", "Tharusha Thejan");
            var subject = "Order Invoice";
            var to = new EmailAddress(order?.User?.Email, order?.User?.FirstName);
            var palinText = "Here's you Invoice";
            var htmlContent = genterateHtmlPayment(order);

            var msg = MailHelper.CreateSingleEmail(from, to, subject, palinText, htmlContent);
            var response = await client.SendEmailAsync(msg);

        }

        public async Task ExecuteReg(User user)
        {
            var client = new SendGridClient("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");
            var from = new EmailAddress("tharushathejanofficial@gmail.com", "Tharusha Thejan");
            var subject = "Registration Successfull";
            var to = new EmailAddress(user?.Email, user?.FirstName);
            var palinText = "Here's you Invoice";
            var htmlContent = GenerateHtmlRegistration(user);

            var msg = MailHelper.CreateSingleEmail(from, to, subject, palinText, htmlContent);
            var response = await client.SendEmailAsync(msg);

        }

        

        public static string genterateHtmlPayment(Order order)
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


            if (order.OrderProduct is not null)
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

        private static string GenerateHtmlRegistration(User user)
        {
            StringBuilder registrationContent = new StringBuilder();

            // Start HTML document
            registrationContent.AppendLine("<!DOCTYPE html>");
            registrationContent.AppendLine("<html lang=\"en\">");
            registrationContent.AppendLine("<head>");
            registrationContent.AppendLine("<meta charset=\"UTF-8\">");
            registrationContent.AppendLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
            registrationContent.AppendLine("<title>Successful Signup</title>");

            // CSS styles
            registrationContent.AppendLine("<style>");
            registrationContent.AppendLine("body { font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px; margin: 0; }");
            registrationContent.AppendLine(".email-container { max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px; background-color: #fff; }");
            registrationContent.AppendLine(".header { background-color: #28a745; color: #ffffff; padding: 20px; text-align: center; border-top-left-radius: 10px; border-top-right-radius: 10px; }");
            registrationContent.AppendLine(".header h1 { margin: 0; }");
            registrationContent.AppendLine(".content { padding: 20px; text-align: center; }");
            registrationContent.AppendLine(".content h2 { color: #333333; }");
            registrationContent.AppendLine(".content p { font-size: 16px; line-height: 1.5; color: #555; }");
            registrationContent.AppendLine(".button { display: inline-block; margin-top: 20px; padding: 10px 20px; color: #ffffff; background-color: #28a745; text-decoration: none; border-radius: 5px; }");
            registrationContent.AppendLine(".footer { background-color: #f4f4f4; color: #666666; padding: 20px; text-align: center; font-size: 12px; border-bottom-left-radius: 10px; border-bottom-right-radius: 10px; }");
            registrationContent.AppendLine(".footer a { color: #28a745; text-decoration: none; }");
            registrationContent.AppendLine("</style>");

            registrationContent.AppendLine("</head>");
            registrationContent.AppendLine("<body>");

            // Registration email content
            registrationContent.AppendLine("<div class='email-container'>");
            registrationContent.AppendLine("<div class='header'>");
            registrationContent.AppendLine("<h1>Signup Successful!</h1>");
            registrationContent.AppendLine("</div>");
            registrationContent.AppendLine("<div class='content'>");
            registrationContent.AppendLine("<h2>Welcome, " + user.FirstName + "!</h2>");
            registrationContent.AppendLine("<p>We're thrilled to have you join MetaGen. Your account has been successfully created and you're now a part of our shopping community.</p>");
            registrationContent.AppendLine("<p>To get started, simply click the button below to log in to your account and explore the amazing shopping features we offer.</p>");
            registrationContent.AppendLine("<a href='[Login Link]' class='button'>Log In</a>");
            registrationContent.AppendLine("</div>");
            registrationContent.AppendLine("<div class='footer'>");
            registrationContent.AppendLine("<p>&copy; 2024 MetaGen. All rights reserved.</p>");
            registrationContent.AppendLine("<p>Colombo, Mirihan<br>");
            registrationContent.AppendLine("<a href='mailto:support@metagen.com'>support@metagen.com</a></p>");
            registrationContent.AppendLine("</div>");
            registrationContent.AppendLine("</div>");

            // End HTML document
            registrationContent.AppendLine("</body>");
            registrationContent.AppendLine("</html>");

            return registrationContent.ToString();
        }




    }

}
