using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Cart
{
    public class CartItemRequset
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
