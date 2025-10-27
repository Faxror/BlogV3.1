using BlogV3._1.Models;
using BusinesssLayer.Abstrack;
using EntityLayerr.Concrate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BlogV3._1.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBlogServices blogedService;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(IBlogServices blogedService, UserManager<AppUser> userManager)
        {
            this.blogedService = blogedService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var blogList = blogedService.getBlogAll();
            return View(blogList);
        }

        public IActionResult GetBlogPartial()
        {
            var blogList = blogedService.getBlogAll();
            return PartialView("_BlogListPartial", blogList); 
        }

        public IActionResult getBlogListWithFirst()
        {
            return PartialView("_BlogTheLastWrite", blogedService.getBlogWithFirst());

        }

        public async Task<IActionResult> BlogDetails(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            ViewBag.id = id;
            var blogGets = blogedService.GetBlogs(id);

            var author = blogedService.getByAuthorİd(blogGets.AuthorID);
            var comment = blogedService.GetCommentsByPost(blogGets.Id);

            var Models = new getBlogWhitAuthor
            {
                Author = author,
                Blog = blogGets,
                Comments = comment
            };

            return View("BlogDetails", Models);
        }


        public IActionResult About()
        {
            var getAuthor = blogedService.GetAuthor();
            return View(getAuthor);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CommentAdds(int postid, string content)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            if (string.IsNullOrWhiteSpace(content))
                return BadRequest("Yorum boş olamaz.");

            var comment = new Comment
            {
                PostId = postid,
                Content = content,
                CreatedAt = DateTime.Now,
                UserId = user.Id
            };

            blogedService.CommentsAdd(comment);
            return RedirectToAction("BlogDetails", "Home", new { id = postid });
        }


        public IActionResult CommentSection(int postId)
        {
            var comments = blogedService.GetCommentsByPost(postId);

            var vm = new CommentViewModel
            {
                NewComment = new Comment { PostId = postId },
                Comments = comments
            };

            return PartialView("_CommentPartial", vm);
        }

    }
}
