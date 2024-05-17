using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Order
{
    public class PaymentProductDto
    {
      
        public string? ImageUrl { get; set; }
       
        public required string Name { get; set; }

        public double Price { get; set; }

        public int Quantity { get; set; }
        
        public decimal PriceTotal { get; set;}


    }
}
