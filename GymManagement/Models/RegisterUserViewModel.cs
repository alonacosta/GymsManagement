using System.ComponentModel.DataAnnotations;

namespace GymManagement.Models
{
    public class RegisterUserViewModel
    {
        [Required]
        [Display( Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name ="Last name")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }

        [Required]
        [MaxLength(9, ErrorMessage = "The phone number has to contain at least 9 characters")]
        public string PhoneNumber { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string Confirm { get; set; }

        public string RoleName { get; set; }
    }
}
