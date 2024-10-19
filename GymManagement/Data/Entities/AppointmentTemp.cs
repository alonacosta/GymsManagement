namespace GymManagement.Data.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class AppointmentTemp : IEntity
    {
        public int Id { get; set; } 
        
        public Client? Client { get; set; }

        [Display(Name = "Session Name")]
        public string? Name { get; set; }

        [Display(Name = "Start Session")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime StartSession { get; set; }

        [Display(Name = "End Session")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime EndSession { get; set; }

        public int? RemainingCapacity { get; set; }

        public string Duration
        {
            get
            {
                var duration = EndSession - StartSession;
                return $"{(int)duration.TotalHours}h {duration.Minutes}m";
            }
        }
    }
}
