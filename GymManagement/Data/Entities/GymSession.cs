using System.ComponentModel.DataAnnotations;

namespace GymManagement.Data.Entities
{
    public class GymSession : IEntity
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public Session? Session { get; set; }
        public int GymId { get; set; }
        public Gym? Gym { get; set; }

        [Display(Name = "Start Session")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = false)]
        public DateTime StartSession { get; set; }

        [Display(Name = "End Session")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = false)]
        public DateTime EndSession { get; set; }

        [Display(Name = "Places Left")]
        public int Capacity { get; set; }

        public IEnumerable<Appointment>? Appointments { get; set; }

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
