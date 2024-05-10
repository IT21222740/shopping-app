using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs;
using Application.DTOs.Order;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Stripe;
using Stripe.Checkout;
using Stripe.Climate;

public class PaymentService : IPamentService // Corrected the interface name
{
    private readonly IHttpContextAccessor _contextAccessor;
    public PaymentService(IHttpContextAccessor httpContextAccessor)
    {
        _contextAccessor = httpContextAccessor;
    }
    public async Task<string> Checkout(string stripeId, IEnumerable<UserProduct> products,string orderId)
    {
        var lineItems = new List<SessionLineItemOptions>();
        

        foreach (var product in products)
        {
            // Create a new line item for each product
            var lineItem = new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long?)(product.Product.Price * 100), // Assuming the price is in dollars, converting it to cents
                    Currency = "lkr",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = product.Product.Name,
                    }
                },
                Quantity = product.Quantity

            };

            lineItems.Add(lineItem);
        }


        var options = new SessionCreateOptions
        {
            LineItems = lineItems,
            Mode = "payment",
            SuccessUrl = "http://localhost:4242/success",
            CancelUrl = "http://localhost:4242/cancel",
            Customer = stripeId,
            ClientReferenceId = orderId
        };

        var service = new SessionService();
        Session session = await service.CreateAsync(options);
        var paymentId = session.PaymentIntentId;
        Console.WriteLine("Payment Id is"+ paymentId);
       
        return session.Url;
    }

    public async Task<string> RegisterUserToPayment(string email, string name)
    {
        var options = new CustomerCreateOptions
        {
            Name = name,
            Email = email,
        };
        var service = new CustomerService();
        var result = await service.CreateAsync(options);
        return result.Id;
    }

    public async Task<WebHookResponse> webHookHandler()
    {
        var json = await new StreamReader(_contextAccessor.HttpContext.Request.Body).ReadToEndAsync();
        try
        {
            var stripeEvent = EventUtility.ParseEvent(json);



            // Handle the event
            if (stripeEvent.Type == Events.CheckoutSessionCompleted)
            {
                var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                var orderPaid = session?.PaymentStatus == "paid";

                if (orderPaid)
                {
                    var paymentCreation = new PaymentCreation
                    {
                        OrderId = int.Parse(session.ClientReferenceId),
                        TotalAmount = (double)(session.AmountTotal / 100),
                        PaymentId = session.PaymentIntentId,
                        Status = "paid"


                    };

                    Console.WriteLine(paymentCreation);
                    return new WebHookResponse(true,CheckoutResponse:paymentCreation);
                }
                else
                {
                    return new WebHookResponse(false);
                }

                // Then define and call a method to handle the successful payment intent.
                // handlePaymentIntentSucceeded(paymentIntent);



            }
            else
            {
                return new WebHookResponse(false);
            }

        }
        catch (StripeException e)
        {
            return new WebHookResponse(Exception:e);
        }

    }
}
