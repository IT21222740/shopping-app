using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Core.Models
{
    public class User:BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string address { get; set; }

        public UserLogin login { get; set; }

        public ICollection<Order> orders { get; set; }
    }
}
