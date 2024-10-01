using System.ComponentModel.DataAnnotations;

namespace GymManagement.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Full name")]
        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Role { get; set; }
    }
}

