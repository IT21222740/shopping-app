using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Core.Models
{
    public class UserLogin:BaseEntity
    {
        [ForeignKey("User")]
        public int UserId { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public User user { get; set; }
    }
}
