using BusinesssLayer.Abstrack;
using DataAccessLayerr.Abstrack;
using EntityLayerr.Concrate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static System.Net.WebRequestMethods;

namespace BlogV3._1.Controllers
{
    public class AdminController : Controller
    {
        private readonly IBlogServices blogServices;

        public AdminController(IBlogServices blogServices)
        {
            this.blogServices = blogServices;
        }

        public IActionResult Index()
        {
            var blogs = blogServices.getBlogAll();
            
            ViewBag.TotalBlogs = blogs.Count();
            ViewBag.PublishedBlogs = blogs.Count(x => x.status == true);
            ViewBag.DraftBlogs = blogs.Count(x => x.status == false);
            return View(blogs);
        }

        public async Task<IActionResult> Write()
        {
            var blog = blogServices.getBlogAll();
            return View(blog);

        }

        [HttpGet]
        public IActionResult AddWrite()
        {

            List<SelectListItem> valuess = (from x in blogServices.GetAllAuthor()
                                            select new SelectListItem
                                            {
                                                Text = x.AuthorName,
                                                Value = x.AuthorID.ToString()
                                            }).ToList();
            ViewBag.v = valuess;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddWrite(Blogs blogs)
        {


            blogs.BlogTime = DateTime.Now;
            blogs.BlogImage = "https://ornek.com/gorsel.jpg";
            blogs.status = true;


            await Task.Run(() => blogServices.addBlog(blogs));

            TempData["SuccessMessage"] = "Yazı başarıyla eklendi!";
            return RedirectToAction("AddWrite");
        }


        [HttpGet]
        public IActionResult EditBlog(int id)
        {
            List<SelectListItem> valuess = (from x in blogServices.GetAllAuthor()
                                            select new SelectListItem
                                            {
                                                Text = x.AuthorName,
                                                Value = x.AuthorID.ToString()
                                            }).ToList();
            ViewBag.v = valuess;
            var blog = blogServices.GetBlogs(id);
            return View(blog);
        }
        [HttpPost]
        public async Task<IActionResult> EditBlog(Blogs blogs)
        {
            blogs.BlogTime = DateTime.Now;
            await Task.Run(() => blogServices.UpdateBlog(blogs));
            return RedirectToAction("Index", "Admin");
        }

        public IActionResult DeleteBlog(int id)
        {
            blogServices.removeBlog(id);
            return RedirectToAction("Index", "Admin");
        }


        [HttpPost]
        public async Task<IActionResult> AddWriteFalse(Blogs blogs)
        {
            blogs.BlogTime = DateTime.Now;
            blogs.BlogImage = "https://ornek.com/gorsel.jpg";
            blogs.status = false;
            await Task.Run(() => blogServices.addBlog(blogs));
            TempData["SuccessMessage"] = "Yazı başarıyla eklendi!";
            return RedirectToAction("AddWrite");
        }
    }
}
