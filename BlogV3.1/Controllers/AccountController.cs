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
                          <p>Lütfen onaylamak için aşağıdaki bağlantıya tıklayın:</p>
                          <p><a href=""{confirmationLink}"">E-posta onayla</a></p>";

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

        public async Task<IActionResult> ChangePasswordInvalid()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendChangePassword(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Bu e-posta adresine kayıtlı bir kullanıcı bulunamadı.";
                return RedirectToAction("SendChangePassword");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var confirmationLink = Url.Action("ChangePassword","Account",new { email = Email, token = WebUtility.UrlEncode(token) },protocol: HttpContext.Request.Scheme);

            string emailBody = $@"
                <p>Merhaba {user},</p>
                <p>Şifrenizi değiştirmek için aşağıdaki bağlantıya tıklayın:</p>
                <p><a href=""{confirmationLink}"">Şifreyi sıfırla</a></p>
                <p>Bu bağlantı yalnızca bir kez kullanılabilir.</p>";

            var mail = new MailMessage("information@pekova.com.tr",Email, "Blog - Şifre Sıfırlama Onayı", emailBody)
            {
                IsBodyHtml = true
            };

            using (var smtpClient = new SmtpClient("mt-prime-win.guzelhosting.com", 587))
            {
                smtpClient.Credentials = new NetworkCredential("information@pekova.com.tr", "2e3Gd9j3*");
                smtpClient.EnableSsl = true;
                await smtpClient.SendMailAsync(mail);
            }

            TempData["SuccessMessage"] = "E-posta adresinize bir sıfırlama bağlantısı gönderildi.";
            return RedirectToAction("SendChangePassword");
        }


        [HttpGet]
        public async Task<IActionResult> ChangePassword(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
                return BadRequest("Geçersiz bağlantı.");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ViewBag.ErrorMessage = "Kullanıcı bulunamadı.";

                return RedirectToAction("ChangePasswordInvalid", "Account");
            }

            var decodedToken = WebUtility.UrlDecode(token);

            var isValid = await _userManager.VerifyUserTokenAsync(
                user,
                _userManager.Options.Tokens.PasswordResetTokenProvider,
                "ResetPassword",
                decodedToken
            );

            if (!isValid){
                ViewBag.ErrorMessage = "Bu link geçerliliğini yitirmiştir.";
                return View("ChangePasswordInvalid", "Account");


            }

            ViewBag.Email = email;
            ViewBag.Token = token;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ChangePassword(string Email, string Token, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı bulunamadı.");
                return View();
            }
            var decodedToken = WebUtility.UrlDecode(Token);

            var result = await _userManager.ResetPasswordAsync(user, decodedToken, newPassword);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Şifreniz başarıyla değiştirildi. Giriş yapabilirsiniz.";
                return RedirectToAction("Login", "Account");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            ViewBag.Email = Email;
            ViewBag.Token = Token;
            return View();
        }

    }
}
