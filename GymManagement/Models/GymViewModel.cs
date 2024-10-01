using GymManagement.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace GymManagement.Models
{
    public class GymViewModel : Gym
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }
    }
}
