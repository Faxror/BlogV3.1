using BlogV3._1.Models;
using BusinesssLayer.Abstrack;
using DataAccessLayerr.Abstrack;
using EntityLayerr.Concrate;
using EntityLayerr.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Globalization;
using static System.Net.WebRequestMethods;

namespace BlogV3._1.Controllers
{
    [Authorize(Roles = "Yönetici")]
    public class AdminController : Controller
    {
        private readonly IBlogServices blogServices;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly ICategoryServices _categoryServices;

        public AdminController(IBlogServices blogServices, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, ICategoryServices categoryServices)
        {
            this.blogServices = blogServices;
            _userManager = userManager;
            _roleManager = roleManager;
            _categoryServices = categoryServices;
        }

        [Authorize(Roles = "Yönetici")]
        public IActionResult Index()
        {
            var blogs = blogServices.getBlogAll();
            
            ViewBag.TotalBlogs = blogs.Count();
            ViewBag.PublishedBlogs = blogs.Count(x => x.status == true);
            ViewBag.DraftBlogs = blogs.Count(x => x.status == false);
            return View(blogs);
        }


      
        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var viewModel = new UserEditYouViewModel
            {
                Id = user.Id,
                FullName = $"{user.Name} {user.Surname}",
                Email = user.Email,
                IsTwoFactorEnabled = user.TwoFactorEnabled
            };

            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UserEditYouViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            user.Email = model.Email;
            var names = model.FullName.Split(' ', 2);
            user.Name = names[0];
            user.Surname = names.Length > 1 ? names[1] : "";

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
                TempData["Success"] = "Profil bilgileri güncellendi!";
            else
                TempData["Error"] = string.Join(" ", result.Errors.Select(e => e.Description));

            return RedirectToAction("Settings");
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(UserEditYouViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            if (model.NewPassword != model.ConfirmPassword)
            {
                TempData["Error"] = "Yeni şifreler eşleşmiyor.";
                return RedirectToAction("Settings");
            }

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
                TempData["Success"] = "Şifre başarıyla güncellendi.";
            else
                TempData["Error"] = string.Join(" ", result.Errors.Select(e => e.Description));

            return RedirectToAction("Settings");
        }



        [HttpPost]
        public async Task<IActionResult> ToggleTwoFactor()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["Error"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("Settings");
            }

            bool newStatus = !user.TwoFactorEnabled;
            var result = await _userManager.SetTwoFactorEnabledAsync(user, newStatus);

            if (result.Succeeded)
            {
                TempData["Success"] = newStatus
                    ? "İki faktörlü kimlik doğrulama etkinleştirildi."
                    : "İki faktörlü kimlik doğrulama devre dışı bırakıldı.";
            }
            else
            {
                TempData["Error"] = "İşlem başarısız oldu.";
            }

            return RedirectToAction("Settings");
        }





        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            var model = new List<UserWithRolesViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                model.Add(new UserWithRolesViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    Surname = user.Surname,
                    UserName = user.UserName,
                    Email = user.Email,
                    ProfilImage = user.ProfilImage,
                    Roles = roles.ToList()
                });
            }

            return View(model);
        }


        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Account"); 
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View("Error", result.Errors);
        }




        [HttpGet]
        public async Task<IActionResult> UserEdit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Users");

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();


            var userRoles = await _userManager.GetRolesAsync(user);
            var currentRole = userRoles.FirstOrDefault();

      
            var allRoles = await _roleManager.Roles
                .Select(r => new SelectListItem
                {
                    Value = r.Name,
                    Text = r.Name,
                    Selected = r.Name == currentRole 
                })
                .ToListAsync();

            var viewModel = new UserEditViewModel
            {
                Id = user.Id,
                userName = user.UserName,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                AllRoles = allRoles,
                SelectedRole = currentRole
            };

            return View(viewModel);
        }




        [HttpPost]
        public async Task<IActionResult> UserEdit(UserEditViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            if (user == null)
                return NotFound();

            user.UserName = model.userName;
            user.Name = model.Name;
            user.Surname = model.Surname;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;

            await _userManager.UpdateAsync(user);


            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles.Any())
                await _userManager.RemoveFromRolesAsync(user, userRoles);


            if (!string.IsNullOrEmpty(model.SelectedRole))
                await _userManager.AddToRoleAsync(user, model.SelectedRole);

            TempData["SuccessMessage"] = "Kullanıcı bilgileri ve rolü başarıyla güncellendi!";
            return RedirectToAction("Users");
        }


        public async Task<IActionResult> Write()
        {
            var authors = blogServices
             .GetAllAuthor()
             .ToDictionary(a => a.AuthorID, a => a.AuthorName);

            ViewBag.Authors = authors;


            var blogs = blogServices.getBlogAll();

            return View(blogs);
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
            List<SelectListItem> Kategorys = (from x in _categoryServices.GetCategories()
                                            select new SelectListItem
                                            {
                                                Text = x.CategoryName,
                                                Value = x.CategoryId.ToString()
                                            }).ToList();
            ViewBag.v = valuess;
            ViewBag.s = Kategorys;
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
            return RedirectToAction("Write");
        }

        [HttpPost]
        public async Task<IActionResult> AddWriteFalse(Blogs blogs)
        {
            blogs.BlogTime = DateTime.Now;
            blogs.BlogImage = "https://ornek.com/gorsel.jpg";
            blogs.status = false;
            await Task.Run(() => blogServices.addBlog(blogs));
            TempData["SuccessMessage"] = "Yazı başarıyla eklendi!";
            return RedirectToAction("Write");
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
            return RedirectToAction("Write", "Admin");
        }

        public IActionResult DeleteBlog(int id)
        {
            blogServices.removeBlog(id);
            return RedirectToAction("Write", "Admin");
        }


   
        [HttpGet]
        public async Task<IActionResult> statistics(DateTime? startDate, DateTime? endDate)
        {
            var start = startDate ?? new DateTime(DateTime.Now.Year, 1, 1);
            var end = endDate ?? new DateTime(DateTime.Now.Year, 12, 31);

            var monthlyStats = blogServices.GetMonthlyStatistics(start, end);

            var viewModel = new StatisticsViewModel
            {
                StartDate = start,
                EndDate = end,
                MonthlyStats = monthlyStats
            };

            return View(viewModel);
        }
        
        
        public async Task<IActionResult> Categories()
        {
            var categories = await Task.Run(() => _categoryServices.GetCategories());
            return View(categories);
        }


        [HttpGet]
        public IActionResult addCategories()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> addCategories(Category category)
        {
            await Task.Run(() => _categoryServices.CategoryAdd(category));
            return RedirectToAction("Categories", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> updateCategory(int id)
        {
            var category = await Task.Run(() => _categoryServices.GetById(id));
            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> updateCategory(Category category)
        {
            await Task.Run(() => _categoryServices.CategoryUpdate(category));
            return RedirectToAction("Categories", "Admin");
        }

        public async Task<IActionResult> deleteCategory(int id)
        {
            await Task.Run(() => _categoryServices.CategoryDelete(id));
            return RedirectToAction("Categories", "Admin");
        }

        

    }
}
