using System.ComponentModel.DataAnnotations;

namespace GymManagement.Models
{
    public class ConfirmEmailViewModel
    {
        public string userId { get; set; }
        public string token { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string Confirm { get; set; }
    }
}
