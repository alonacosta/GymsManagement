using GymManagement.Data;
using GymManagement.Data.Entities;
using GymManagement.Helpers;
using GymManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace GymManagement.Controllers
{
    public class PostsController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserHelper _userHelper;

        public PostsController(IPostRepository postRepository,
            IUserHelper userHelper)
        {
            _postRepository = postRepository;
            _userHelper = userHelper;
        }

        [Authorize]
        public IActionResult Index()
        {
            var model = _postRepository.GetDiscussionsWithPosts();
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> CreateDiscussion()
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            var model = new CreatePostViewModel
            {
                Username = user.UserName,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDiscussion(CreatePostViewModel model)
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    await _postRepository.CreateAsync(new Discussion
                    {
                        OriginalPost = new Post
                        {
                            User = user,
                            Title = model.Title,
                            Message = model.Message,
                        }
                    });
                }
                else
                {
                    ViewBag.Error = "Something went wrong when creating a discussion thread.";
                }
            } else
            {
                ViewBag.Error = "Something went wrong when creating a discussion thread.";
            }
            var discussionId = _postRepository.GetLastDiscussionIdByUser(user);
            return RedirectToAction($"Discussion", new { id = discussionId }); ;
        }

        public IActionResult Discussion(int? id)
        {
            if (id == null)
            {
                return NotFound();
            } 
            else
            {
                ViewData["DiscussionId"] = id.Value;
                var model = _postRepository.GetDiscussion(id.Value);
                return View(model);
            }
        }

        [Authorize]
        public IActionResult CreateReply(int id)
        {
            var model = new CreatePostViewModel
            {
                DiscussionId = id,
                Username = this.User.Identity.Name
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReply(CreatePostViewModel model)
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            if (ModelState.IsValid)
            {
                await _postRepository.PostReply(model, user);
            }
            return RedirectToAction($"Discussion", new { id = model.DiscussionId });
        }

        [Authorize]
        public async Task<IActionResult> UpdatePostAsync(int id)
        {
            var post = await _postRepository.GetPostByIdAsync(id);
            var model = new ChangePostViewModel
            {
                PostId = id,
                Username = post.User.Email,
                Message = post.Message
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePostAsync(ChangePostViewModel model)
        {            
            var post = await _postRepository.GetPostByIdAsync(model.PostId);
            if (post != null)
            {
                post.Message = model.Message;
                await _postRepository.UpdatePostAsync(post);                
            }
            return View(model);
        }

        public async Task<IActionResult> DeleteReply(int id) 
        {
            var discussionId = await _postRepository.GetDiscussionIdByReplyAsync(id);
            try
            {
                await _postRepository.DeletePostAsync(id);
                if (discussionId != null)
                {
                    return RedirectToAction("Discussion",new { id = (int)discussionId });
                }
                else 
                {
                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                {
                    ViewBag.ErrorTitle = $"The post couldn't be removed.";
                }
                return View("Error");
            }
        }

        public async Task<IActionResult> DeleteDiscussion(int id)
        { 
            var discussion = await _postRepository.GetByIdAsync(id);
            try
            {
                await _postRepository.DeleteAsync(discussion);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                {
                    ViewBag.ErrorTitle = $"Discussion wasn't able to be deleted";
                    ViewBag.ErrorMessage = $"This discussion wasn't able to be deleted, probably because" +
                        $"it contains replies. If you are an administrator, try to remove said replies" +
                        $"before trying to remove a discussion thread.";
                }
                return View("Error");
            }
        }
    }
}
