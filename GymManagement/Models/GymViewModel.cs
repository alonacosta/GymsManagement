using GymManagement.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace GymManagement.Models
{
    public class GymViewModel : Gym
    {
        [Display(Name = "Image")]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "City")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a city.")]
        public int CityId { get; set; }

        //public string? CityName { get; set; }

        public IEnumerable<SelectListItem>? Cities { get; set; }

        [Display(Name = "Country")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a country.")]
        public int? CountryId { get; set; }

       
        public IEnumerable<SelectListItem>? Countries { get; set; }
    }
}
