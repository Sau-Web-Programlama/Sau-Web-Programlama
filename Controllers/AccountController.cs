// Controllers/AccountController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace FitnessCenter.Controllers
{
    public class AccountController : Controller
    {
        // Giriþ sayfasý
        public IActionResult Login()
        {
            return View();
        }

        // Giriþ iþlemi
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Veritabanýndan kullanýcý kontrolü
                // Demo için basit kontrol
                if (model.Email == "ogrencinumarasi@sakarya.edu.tr" && model.Password == "sau")
                {
                    // Admin giriþi
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.Email),
                        new Claim(ClaimTypes.Email, model.Email),
                        new Claim(ClaimTypes.Role, "Admin")
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    return RedirectToAction("Index", "Admin");
                }
                else if (model.Email == "uye@sakarya.edu.tr" && model.Password == "123456")
                {
                    // Üye giriþi
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.Email),
                        new Claim(ClaimTypes.Email, model.Email),
                        new Claim(ClaimTypes.Role, "Member")
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "E-posta veya þifre hatalý!");
                }
            }

            return View(model);
        }

        // Kayýt sayfasý
        public IActionResult Register()
        {
            return View();
        }

        // Kayýt iþlemi
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Þifre kontrolü
                if (model.Password != model.ConfirmPassword)
                {
                    ModelState.AddModelError("", "Þifreler eþleþmiyor!");
                    return View(model);
                }

                // Veritabanýna kaydet
                // E-posta kontrolü yap
                // Baþarýlý kayýt mesajý

                TempData["Success"] = "Kayýt baþarýlý! Giriþ yapabilirsiniz.";
                return RedirectToAction("Login");
            }

            return View(model);
        }

        // Çýkýþ iþlemi
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }

    // ViewModels (þimdilik burada, Models klasöründe olacak)
    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool AcceptTerms { get; set; }
    }
}