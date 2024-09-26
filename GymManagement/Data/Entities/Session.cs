using System;
using System.ComponentModel.DataAnnotations;

namespace GymManagement.Data.Entities
{
    public class Session : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        //public Client Client { get; set; }

        [Display(Name = "Start Session")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime StartSession { get; set; }

        [Display(Name = "End Session")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime EndSession { get; set; }

        public int Capacity { get; set; }   

        public bool isOnline { get; set; }
        public bool isGroup { get; set; }
        public Gym Gym { get; set; }

        public IEnumerable<Appointment> Appointments { get; set; }       

        public int RemainingCapacity => Appointments == null ? 100 : Capacity - Appointments.Count();

        [Display(Name = "Start Session")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime? StartSessionLocal => this.StartSession == null ? null : this.StartSession.ToLocalTime();

        [Display(Name = "End Session")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime? EndSessionLocal => this.EndSession == null ? null : this.EndSession.ToLocalTime();
    }
}
