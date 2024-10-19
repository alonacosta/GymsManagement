namespace GymManagement.Data.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class Gym : IEntity
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Gym Name")]
        public string? Name { get; set; }

        [Required]
        public string? Address { get; set; }

        [Display(Name = "Image")]
        public Guid ImageId { get; set; }

        [Display(Name = "City")]
        public int CityId { get; set; }

        public City? City { get; set; }

        public ICollection<Client>? Clients { get; set; } = new List<Client>();
        public ICollection<Employee>? Employees { get; set; } = new List<Employee>();

        //public string? CityName => City.Name.ToString();

        public string ImageFullPath => ImageId == Guid.Empty
            ? "https://gymmanagement.blob.core.windows.net/default/no-image.jpeg" 
            : $"https://gymmanagement.blob.core.windows.net/gyms/{ImageId}";
    }
}
