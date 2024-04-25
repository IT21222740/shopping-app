using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class User
    {
        [Key]
        public string Id { get; set; }

        public string? Name { get; set; }

        [Required]
        public string email { get; set; }

        /*
        //Address(1:N)
        public ICollection<Address>? Addresses { get; set; }

        //Payments(1:N)
        public ICollection<Payment>? Payments { get; set; }

        public ICollection<Order>? Orders { get; set; }
        */
    }
}
