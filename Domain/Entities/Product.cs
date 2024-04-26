using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public required string Name { get; set; }

        public double Price { get; set; }

        public int Quantity { get; set; }
        public double Discount { get; set; }

        public virtual ProductCategory? ProductCategory { get; set; }

        public int ProductCategoryId { get; set; }
        
        public virtual ICollection<OrderProduct>? OrderProduct { get; set; }
        public ICollection<UserProduct>? UserProducts { get; set; }



    }
}
