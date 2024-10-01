using System.ComponentModel.DataAnnotations;

namespace GymManagement.Models
{
    public class EditUserViewModel
    {
        [Required]
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} caraters.")]
        public string? FirstName { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} caraters.")]
        public string? LastName { get; set; }

        public string Username { get; set; }
    }
}
