using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }

        public double TotalAmount { get; set; }
        public string? Status { get; set; }

        public string? PaymentId { get; set; }

        public virtual User? User { get; set; }
        public required string UserId { get; set; }
        public virtual ICollection<OrderProduct>? OrderProduct { get; set; }
    }
}
