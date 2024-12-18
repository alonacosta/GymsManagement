using GymManagement.Data.Entities;
using GymManagement.Models;

namespace GymManagement.Data
{
    public interface IPostRepository : IGenericRepository<Discussion>
    {
        IQueryable GetDiscussionsWithPosts();

        Discussion GetDiscussion(int? id);

        Task PostReply(CreatePostViewModel model, User user);

        Task<Discussion> GetDiscussionByPost(int? id);

        Task<Post> GetPostByIdAsync(int id);

        Task UpdatePostAsync(Post post);

        Task DeletePostAsync(int id);

        Task<int?> GetDiscussionIdByReplyAsync(int id);

        int? GetLastDiscussionIdByUser(User user);
    }
}