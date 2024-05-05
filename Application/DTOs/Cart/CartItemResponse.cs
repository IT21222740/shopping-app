using Application.DTOs.Product;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Cart
{
    public class CartItemResponse
    {

        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public ProductCartResponse? Product { get; set; }

    }
}
