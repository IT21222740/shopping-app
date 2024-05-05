using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Stripe;
using Stripe.Checkout;

public class PaymentService : IPamentService // Corrected the interface name
{
    public async Task<string> Checkout(string stripeId, IEnumerable<UserProduct> products)
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
            Customer = stripeId
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
}
