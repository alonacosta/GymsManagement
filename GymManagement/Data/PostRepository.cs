using GymManagement.Data.Entities;
using GymManagement.Helpers;
using GymManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Data
{
    public class PostRepository : GenericRepository<Discussion>, IPostRepository
    {
        private readonly DataContext _context;

        public PostRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable GetDiscussionsWithPosts()
        {
            return _context.Discussions
                .Include(d => d.OriginalPost)
                .OrderBy(d => d.Id);
        }

        public Discussion GetDiscussion(int? id)
        {
            return _context.Discussions
                .Where(d => d.Id == id)
                .Include(d => d.OriginalPost)
                .Include(d => d.OriginalPost.User)
                .Include(d => d.Replies)
                .FirstOrDefault();
        }


        public async Task PostReply(CreatePostViewModel model, User user)
        {
            var discussion =  await _context.Discussions
                .Where(d => d.Id == model.DiscussionId)
                .Include(d => d.OriginalPost)
                .Include(d => d.Replies)
                .FirstOrDefaultAsync();

            if (discussion != null) 
            {
                discussion.Replies.Add(new Post
                {
                    Title = discussion.OriginalPost.Title,
                    Message = model.Message,
                    User = user,
                });
                
            }
            await _context.SaveChangesAsync();
        }

        public async Task<Post> GetPostByIdAsync(int id)
        {
            return await _context.Posts.Where(p => p.Id == id)
                .Include(p => p.User)
                .FirstOrDefaultAsync();
        }

        public async Task UpdatePostAsync(Post post)
        {
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePostAsync(int id)
        {
            var post = await GetPostByIdAsync(id);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }

        public async Task<int?> GetDiscussionIdByReplyAsync(int id)
        {
            var discussion = await _context.Discussions
                .Where(d => d.Replies.Any(r => r.Id == id))
                .FirstOrDefaultAsync();

            return discussion.Id;
        }

        public int? GetLastDiscussionIdByUser(User user)
        {
            return _context.Discussions
                .Where(d => d.OriginalPost.User.Id == user.Id)
                .OrderBy(d => d.Id)
                .LastOrDefault().Id;
        }
    }
}