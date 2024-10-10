using GymManagement.Data.Entities;

namespace GymManagement.Models
{
    public class DiscussionViewModel
    {
        public int AuthorId {  get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public ICollection<Post> Replies { get; set; }
    }
}
