namespace GymManagement.Data.Entities
{
    public class Appointment : IEntity
    {
        public int Id { get; set; }

        public Session? Session { get; set; }
        public GymSession GymSession { get; set; }        
        public Client Client { get; set; }
    }
}
