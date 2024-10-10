using GymManagement.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace GymManagement.Models
{
    public class CreatePostViewModel
    {
        public int DiscussionId { get; set; } 

        [Required]
        public string Username{ get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = "The title has a limit of {0} characters.")]
        public string Title { get; set; }

        [Required]
        [MaxLength(1024, ErrorMessage = "The post has a limit of {0} characters.")]
        public string Message { get; set; }

    }
}
