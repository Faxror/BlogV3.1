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
            return View();
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


        [HttpDelete]
        public IActionResult DeleteBlog(int id)
        {
            blogServices.removeBlog(id);
            return RedirectToAction("Index", "Home");
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
