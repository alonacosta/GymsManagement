namespace GymManagement.Data.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class Country : IEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "The field {0} can only contain {1} characters.")]
        public string? Name { get; set; }

        [Required]
        public ICollection<City>? Cities { get; set; }

        [Display(Name = "Number of cities")]
        public int NumberCities => Cities == null ? 0 : Cities.Count;
    }
}
