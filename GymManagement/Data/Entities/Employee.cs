using System.ComponentModel.DataAnnotations;

namespace GymManagement.Data.Entities
{
    public class Employee :IEntity
    {
        public int Id { get; set; }

        [Required]
        public User User { get; set; }
    }
}
