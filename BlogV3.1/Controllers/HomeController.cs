using System.Diagnostics;
using BlogV3._1.Models;
using BusinesssLayer.Abstrack;
using Microsoft.AspNetCore.Mvc;

namespace BlogV3._1.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBlogServices blogedService;

        public HomeController(IBlogServices blogedService)
        {
            this.blogedService = blogedService;
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

        public IActionResult BlogDetails(int id)
        {
            ViewBag.id = id;
            var blogGets = blogedService.GetBlogs(id);

            var author = blogedService.getByAuthorİd(blogGets.AuthorID);

            var Models = new getBlogWhitAuthor
            {
                Author = author,
                Blog = blogGets
            };

            return View("BlogDetails", Models);
        }


        public IActionResult About()
        {
            var getAuthor = blogedService.GetAuthor();
            return View(getAuthor);
        }
    }
}
