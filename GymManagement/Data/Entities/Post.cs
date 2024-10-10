namespace GymManagement.Data.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class Post : IEntity
    {
        public int Id { get; set; } 
        
        public string? Title { get; set; }

        [Required]
        public string? Message { get; set; }

        [Required]
        public User? User { get; set; }

    }
}
