using SporSalonu2.Models;
using SporSalonu2.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization; // [Authorize] için gerekli
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Async veritabanı işlemleri için gerekli
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SporSalonu2.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                ClaimsIdentity claimsIdentity = null;
                bool isAuthenticated = false;

                if (model.Email == "ogrencinumarasi@sakarya.edu.tr" && model.Password == "sau")
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.Email),
                        new Claim(ClaimTypes.Email, model.Email),
                        new Claim(ClaimTypes.Role, "Admin")
                    };
                    claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    isAuthenticated = true;
                }
                else
                {
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);

                    if (user != null)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Email),
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim(ClaimTypes.Role, user.Role)
                        };
                        claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        isAuthenticated = true;
                    }
                }

                if (isAuthenticated && claimsIdentity != null)
                {
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    if (claimsIdentity.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "Admin"))
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "E-posta veya şifre hatalı!");
                }
            }

            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (model.AcceptTerms == false)
            {
                ModelState.AddModelError("AcceptTerms", "Kullanım şartlarını kabul etmelisiniz.");
            }

            if (ModelState.IsValid)
            {
                if (await _context.Users.AnyAsync(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("", "Bu e-posta adresi zaten kayıtlı.");
                    return View(model);
                }

                var newUser = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = model.Password,
                    Role = "Member",
                    Phone = model.Phone 
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Kayıt başarılı! Lütfen giriş yapınız.";
                return RedirectToAction("Login");
            }

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var userEmail = User.Identity.Name;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user == null) return NotFound();

            var model = new UserProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                Height = user.Height,
                Weight = user.Weight
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(UserProfileViewModel model)
        {
            var userEmail = User.Identity.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user != null)
            {
                user.Phone = model.Phone;
                user.Height = model.Height;
                user.Weight = model.Weight;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Profil bilgileriniz başarıyla güncellendi.";
            }

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccount()
        {
            var userEmail = User.Identity.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user != null)
            {
                // Kullanıcıya ait randevuları sil (Veritabanı bütünlüğü için)
                var bookings = _context.Bookings.Where(b => b.MemberId == user.Email);
                _context.Bookings.RemoveRange(bookings);

                // Kullanıcıyı sil
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                // Oturumu kapat
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}