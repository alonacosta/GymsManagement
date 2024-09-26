using System.ComponentModel.DataAnnotations;

namespace GymManagement.Data.Entities
{
    public class Client : IEntity
    {
        public int Id { get; set; }

        [Required]
        public User User { get; set; }

        public IEnumerable<Appointment> Appointments { get; set; }
    }
}
