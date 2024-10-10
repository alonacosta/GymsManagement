using System.ComponentModel.DataAnnotations;

namespace GymManagement.Models
{
    public class ChangePostViewModel
    {
        public int PostId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [MaxLength(400, ErrorMessage = "The post has a limit of {0} characters.")]
        public string Message { get; set; }
    }
}
