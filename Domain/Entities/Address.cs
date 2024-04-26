using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Address
    {
        public int AddressId { get; set; }
        public required string City { get; set; }
        public required string StreetName { get; set; }
        public int PostalCode { get; set; }

        //User(1:N)
        public virtual User? User { get; set; }

        public required string UserId { get; set; }
    }
}
