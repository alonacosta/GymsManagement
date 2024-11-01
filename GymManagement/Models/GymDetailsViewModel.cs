using GymManagement.Data.Entities;

namespace GymManagement.Models
{
    public class GymDetailsViewModel
    {
        public int GymId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }

        public int CityId { get; set; }
        public City? City { get; set; }

        public Guid ImageGymId { get; set; }
        public string? ImageGymUrl { get; set; }

        public string FullName { get; set; }
        
        public ICollection<Employee>? Employees { get; set; }
        

    }
}
