namespace GymManagement.Models
{
    public class AppointmentData
    {
        public int Id { get; set; } 
        public string Subject { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool? IsReadonly { get; set; }

        public string PrimaryColor { get; set; }
        public string SecondaryColor { get; set; }

      
       
    }
}
