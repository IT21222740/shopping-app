using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserProduct
    {
        public virtual User? User { get; set; }
        public required string userId { get; set; }
        public virtual Product? Product { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

    }
}
