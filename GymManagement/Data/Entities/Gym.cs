using System.ComponentModel.DataAnnotations;

namespace GymManagement.Data.Entities
{
    public class Gym : IEntity
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public ICollection<Client> Clients { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
