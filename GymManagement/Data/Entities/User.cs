using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;


namespace GymManagement.Data.Entities
{
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} caraters.")]
        public string? FirstName { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} caraters.")]
        public string? LastName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";
    }
}
