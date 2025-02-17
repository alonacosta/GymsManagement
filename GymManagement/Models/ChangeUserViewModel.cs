﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace GymManagement.Models
{
    public class ChangeUserViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        [MaxLength(50, ErrorMessage = "The field {0} can only contain {1} caracters.")]
        public string? FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [MaxLength(50, ErrorMessage = "The field {0} can only contain {1} caracters.")]
        public string? LastName { get; set; }

        [Required]
        [MaxLength(9, ErrorMessage = "The phone number has to contain at least 9 characters")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Image")]
        public Guid ImageId { get; set; }

        public string? ImagePath { get; set; }

        [Display(Name = "Image")]
        public IFormFile? ImageFile { get; set; }
    }
}

