namespace GymManagement.Data.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class Employee : IEntity
    {
        public int Id { get; set; }

        [Required]
        public User? User { get; set; }
    }
}
