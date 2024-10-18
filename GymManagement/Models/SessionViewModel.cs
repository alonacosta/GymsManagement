namespace GymManagement.Models
{
    using GymManagement.Data.Entities;
    using System.ComponentModel.DataAnnotations;

    public class SessionViewModel : Session
    {
        [Display(Name ="Image")]
        public IFormFile? ImageFile { get; set; }
    }
}
