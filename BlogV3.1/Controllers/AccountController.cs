using BlogV3._1.Models;
using DataAccessLayer.Concrete;
using EntityLayerr.Concrate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

namespace BlogV3._1.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        private readonly DBContext dBContext;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, DBContext dBContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.dBContext = dBContext;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            var result = await _signInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password, false, true);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(loginViewModel.UserName);
                if (user.EmailConfirmed)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["Mail"] = user.Email;
                    ViewBag.email = user.Email;
                    return RedirectToAction("EmailWiat", "Account");
                }
            }
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                Random random = new Random();
                int confirimCode = random.Next(100000, 999999);
                AppUser appUser = new AppUser
                {
                    UserName = registerViewModel.Username,
                    Email = registerViewModel.Email,
                    Name = registerViewModel.Name,
                    Surname = registerViewModel.Surname,
                    Phone = registerViewModel.Phone,
                    ConfirimCode = confirimCode

                };

                var result = await _userManager.CreateAsync(appUser, registerViewModel.Password);
                if (result != null && result.Succeeded)
                {
                    SmtpClient smtpClient = new SmtpClient("mt-prime-win.guzelhosting.com", 587);
                    smtpClient.Credentials = new NetworkCredential("information@pekova.com.tr", "2e3Gd9j3*");
                    smtpClient.EnableSsl = true;

                    string recipientEmail = $"{appUser.Email}";


                    string emailTitle = "Blog - E posta onay kodu";


                    string confirmationLink = $"{Url.Action("EmailConfirme", "Account", new { email = appUser.Email, code = confirimCode }, protocol: HttpContext.Request.Scheme)}";

                    string emailBody = $@"<p>Merhaba,</p>
                          <p>Kayıt İşlemini Bitirmek İçin kodunuz: {confirimCode}.</p>
                          <p>Lütfen onaylamak için aşağıdaki bağlantıya tıklayın:</p>
                          <p><a href=""{confirmationLink}"">{confirmationLink}</a></p>";

                    MailMessage mail = new MailMessage("information@pekova.com.tr", recipientEmail, emailTitle, emailBody);


                    mail.IsBodyHtml = true;

                    await smtpClient.SendMailAsync(mail);


                    TempData["Mail"] = appUser.Email;
                    TempData["Code"] = confirimCode;
                    ViewBag.email = appUser.Email;
                    ViewBag.Code = confirimCode;
                    return RedirectToAction("EmailWiat", "Account");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            else
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"[VALIDATION ERROR] {state.Key}: {error.ErrorMessage}");
                    }
                }


            }
            return View();
        }

        public IActionResult EmailWiat()
        {
            return View();
        }

        [HttpGet]
        public IActionResult EmailConfirme()
        {
            var value = TempData["Mail"];
            var code = TempData["Code"];
            ViewBag.email = value;
            ViewBag.Code = code;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EmailConfirme(string email, int code)
        {
            if (email == null || code == 0)
            {
                return RedirectToAction("Login", "Account");
            }
            var user = dBContext.Users.FirstOrDefault(u => u.Email == email && u.ConfirimCode == code);
            if (user != null)
            {
                user.ConfirimCode = 0;
                user.EmailConfirmed = true;
                dBContext.SaveChanges();
                return RedirectToAction("Login", "Account");
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult SendChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendChangePassword(string Email)
        {
            SmtpClient smtpClient = new SmtpClient("mt-prime-win.guzelhosting.com", 587);
            smtpClient.Credentials = new NetworkCredential("information@pekova.com.tr", "2e3Gd9j3*");
            smtpClient.EnableSsl = true;

            string recipientEmail = $"{Email}";


            string emailTitle = "Blog - Şifre değiştirme onay";


            string confirmationLink = $"{Url.Action("ChangePassword", "Account", new { email = Email }, protocol: HttpContext.Request.Scheme)}";

            string emailBody = $@"<p>Merhaba,</p>
                          <p>Lütfen onaylamak için aşağıdaki bağlantıya tıklayın:</p>
                          <p><a href=""{confirmationLink}"">{confirmationLink}</a></p>";

            MailMessage mail = new MailMessage("information@pekova.com.tr", recipientEmail, emailTitle, emailBody);


           mail.IsBodyHtml = true;

            await smtpClient.SendMailAsync(mail);


            TempData["Mail"] = Email;
            ViewBag.email = Email;
            return RedirectToAction("SendChangePassword", "Account");
               
        }


        [HttpGet]
        public IActionResult ChangePassword(string email)
        {
            ViewBag.Email = email;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string Email, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user != null)
            {

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
                if (result.Succeeded)
                {
                    TempData["NewPassword"] = newPassword;
                    ViewBag.NewPassword = newPassword;
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Kullanıcı bulunamadı.");
            }
            return View();
        }
    }
}
