namespace GymManagement.Data.Entities
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser
    {
        [Required]
        [Display(Name ="First Name")]
        [MaxLength(50, ErrorMessage = "The field {0} can only contain {1} caracters.")]
        public string? FirstName { get; set; }

        [Required]
        [Display(Name ="Last Name")]
        [MaxLength(50, ErrorMessage = "The field {0} can only contain {1} caracters.")]
        public string? LastName { get; set; }

        public Guid ImageId { get; set; }

        [Display(Name ="Full Name")]
        public string FullName => $"{FirstName} {LastName}";

        public string ImageUserFullPath => ImageId == Guid.Empty
            ? "https://gymmanagement.blob.core.windows.net/default/no-profile.png"
            : $"https://gymmanagement.blob.core.windows.net/users/{ImageId}";


    }
}
