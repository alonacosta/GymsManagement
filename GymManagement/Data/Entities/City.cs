namespace GymManagement.Data.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class City : IEntity
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "City")]
        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters.")]
        public string? Name { get; set; }

        public ICollection<Gym>? Gyms { get; set; }
        public Country? Country { get; set; }

        [Display(Name = "Number of gyms")]
        public int NumberGyms => Gyms == null ? 0 : Gyms.Count;
    }
}
