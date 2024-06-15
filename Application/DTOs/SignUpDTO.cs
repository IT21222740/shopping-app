using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class SignUpDTO
    {
        [Required(ErrorMessage = "Email is Required")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]{1,64}@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}",
            ErrorMessage = "Your email is not valid. Pleases enter a valid email. ")]
        public required string Email { get; set; }
        
        [Required(ErrorMessage ="password in Required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])(?=.{8,16}).*$",
            ErrorMessage ="Password must contain atleast 8 charactes and no more that 16 characters with One Uppercase, one lowercase, one digit and one special character")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [RegularExpression(@"^[a-zA-Z]{1,40}$", ErrorMessage = "First name must be between 1 and 40 characters and contain only letters.")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [RegularExpression(@"^[a-zA-Z]{1,40}$", ErrorMessage = "Last name must be between 1 and 40 characters and contain only letters.")]
        public required string LastName { get; set; }

        [Required(ErrorMessage ="Phone number is necessary")]
        [RegularExpression(@"^[\d]{10}$")]
        public required string PhoneNumber { get; set; }

        public required AddressDTO Address { get; set; }

    }
}
