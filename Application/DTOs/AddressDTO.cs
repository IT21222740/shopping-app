using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class AddressDTO
    {
        
        public required string City { get; set; }
        public required string StreetName { get; set; }
        public int PostalCode { get; set; }
    }
}
