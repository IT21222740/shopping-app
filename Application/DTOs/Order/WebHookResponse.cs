using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Order
{
    public record WebHookResponse(bool? PaymentStatus = null, PaymentCreation? CheckoutResponse =null, Exception? Exception = null);
}
