namespace GymManagement.Data.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class Gym : IEntity
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Address { get; set; }

        public ICollection<Client>? Clients { get; set; }

        public ICollection<Employee>? Employees { get; set; }
    }
}
