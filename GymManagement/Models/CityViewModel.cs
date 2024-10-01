namespace GymManagement.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CityViewModel
    {
        [Required]
        [Display(Name = "City")]
        [MaxLength(50, ErrorMessage ="The field {0} can only contain {1} characters.")]
        public string? Name { get; set; }

        public int CountryId { get; set; }

        public int CityId { get; set; }
    }
}
