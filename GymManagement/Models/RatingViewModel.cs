using GymManagement.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagement.Models
{
    public class RatingViewModel : Rating
    {
        public int SelectedRating { get; set; }
        public IEnumerable<SelectListItem>? Ratings { get; set; }
        public int? AppointmentId { get; set; }
    }
}
