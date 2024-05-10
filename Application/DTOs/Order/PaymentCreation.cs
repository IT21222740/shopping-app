using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Order
{
    public class PaymentCreation
    {
        public int OrderId { get; set; }
        public double TotalAmount { get; set; }
        public string PaymentId { get; set; }
        public string Status { get; set; }
    }
}
