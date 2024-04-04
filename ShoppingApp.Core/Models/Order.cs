using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Core.Models
{
    public class Order: BaseEntity
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public int userId {  get; set; }
        public User user { get; set; }

        public ICollection<Products> Products { get; set; }
        public double Total { get; set; }
    }
}
