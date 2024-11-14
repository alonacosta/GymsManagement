namespace GymManagement.Data.Entities
{
    public class Rating : IEntity
    {
        public int Id { get; set; }
        public int GymSessionId { get; set; }
        public GymSession? GymSession { get; set; }
        public string? UserId { get; set; }
        public int Rate { get; set; }
    }
}
