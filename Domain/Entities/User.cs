using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class User
    {
        [Key]
        public required string UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? StripeId { get; set; }
        [Required]
        public required string Email { get; set; }

        public string? PhoneNumber {  get; set; }
        //Address(1:N)
        public ICollection<Address>? Addresses { get; set; }

        public ICollection<Order>? Orders { get; set; }
        public ICollection<UserProduct>? UserProducts { get; set; }
        
    }
}
