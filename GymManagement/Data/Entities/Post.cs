﻿using System.ComponentModel.DataAnnotations;

namespace GymManagement.Data.Entities
{
    public class Post : IEntity
    {
        public int Id { get; set; }       
        public string Title { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public User User { get; set; }

        [Required]
        public Gym Gym { get; set; }

    }
}
