using System.ComponentModel.DataAnnotations;

namespace GymManagement.Models
{
    public class GymSessionRatingViewModel
    {
        //public int? GymSessionId { get; set; }

        public int GymId {  get; set; }

        //public string? SessionName { get; set; }

        [Display(Name = "Rating")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double AverageRating { get; set; }

        [Display(Name = "Session Name")]
        public string? GymSessionName { get; set; }
    }
}
