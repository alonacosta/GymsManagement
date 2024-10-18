using System.ComponentModel.DataAnnotations;

namespace GymManagement.Models
{
    public class RecoverPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
