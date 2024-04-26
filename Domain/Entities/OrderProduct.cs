using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class OrderProduct
    {
        public virtual Order? Order { get; set; }
        public int OrderId { get; set; }
        public virtual Product? Product { get; set; }
        public int ProductId { get; set; }
        public int OrderQty { get; set; }
        public double UnitPrice { get; set; }
    }
}
