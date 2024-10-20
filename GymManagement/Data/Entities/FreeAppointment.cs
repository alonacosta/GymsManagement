using System.ComponentModel.DataAnnotations;

namespace GymManagement.Data.Entities
{
    public class FreeAppointment : IEntity
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [MaxLength(50, ErrorMessage = "The field {0} can only contain {1} caracters.")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [MaxLength(50, ErrorMessage = "The field {0} can only contain {1} caracters.")]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Session you want")]
        public string SessionName { get; set; }

        [Display(Name = "Message")]
        public string? Message { get; set; }

        public bool IsComplete { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";

    }
}
