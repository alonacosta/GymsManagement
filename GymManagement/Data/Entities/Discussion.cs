namespace GymManagement.Data.Entities
{
    public class Discussion : IEntity
    {
        public int Id { get; set; }

        public Post OriginalPost { get; set; } 

        public ICollection<Post> Replies { get; set; } 
    }
}
