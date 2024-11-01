namespace GymManagement.Data.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class Employee : IEntity
    {
        public int Id { get; set; }

        [Required]
        public User? User { get; set; }

        public Gym? Gym { get; set; }

        public Position? Position { get; set; }
        public int? PositionId { get; set; } 
    }
}
