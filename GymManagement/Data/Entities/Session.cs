﻿namespace GymManagement.Data.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Session : IEntity
    {
        public int Id { get; set; }

        public string? Name { get; set; }
        
        //public Client Client { get; set; }

        [Display(Name = "Start Session")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime? StartSession { get; set; }

        [Display(Name = "End Session")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime? EndSession { get; set; }

        [Display(Name = "Places Left")]
        public int? Capacity { get; set; }

        [Display(Name ="Is Online")]
        public bool IsOnline { get; set; }

        [Display(Name ="Is Group")]
        public bool IsGroup { get; set; }

        public Gym? Gym { get; set; }

        [Display(Name ="Image")]
        public Guid ImageId { get; set; }

        public IEnumerable<Appointment>? Appointments { get; set; }
        //public int RemainingCapacity => Appointments == null ? 100 : Capacity - Appointments.Count();

        //[Display(Name = "Start Session")]
        //[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm tt}", ApplyFormatInEditMode = false)]
        //public DateTime? StartSessionLocal => this.StartSession == null ? null : this.StartSession.ToLocalTime();

        //[Display(Name = "End Session")]
        //[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm tt}", ApplyFormatInEditMode = false)]
        //public DateTime? EndSessionLocal => this.EndSession == null ? null : this.EndSession.ToLocalTime();

        //public string Duration
        //{
        //    get
        //    {
        //        var duration = EndSession - StartSession;
        //        return $"{(int)duration.TotalHours}h {duration.Minutes}m";
        //    }
        //}

        public string ImageFullPath => ImageId == Guid.Empty
          ? "https://gymmanagement.blob.core.windows.net/default/no-image.jpeg"
          : $"https://gymmanagement.blob.core.windows.net/sessions/{ImageId}";
    }
}
