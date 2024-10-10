namespace GymManagement.Data.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class Client : IEntity
    {
        public int Id { get; set; }

        [Required]
        public User? User { get; set; }

        public Gym Gym { get; set; }
        public IEnumerable<Appointment>? Appointments { get; set; }
    }
}
