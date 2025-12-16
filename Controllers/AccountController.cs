using SporSalonu2.Models;
using SporSalonu2.Data; // DbContext için gerekli
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Linq; // LINQ sorguları için gerekli
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FitnessCenter.Controllers
{
    public class AccountController : Controller
    {
        // ---------------------------------------------------------
        // 1. EKSİK OLAN KISIM: Veritabanı Bağlantısı (Dependency Injection)
        // ---------------------------------------------------------
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }
        // ---------------------------------------------------------

        // Giriş sayfası
        public IActionResult Login()
        {
            return View();
        }

        // Giriş işlemi
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                ClaimsIdentity claimsIdentity = null;
                bool isAuthenticated = false;

                // A) ÖDEV GEREĞİ: Sabit Admin Kontrolü
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
                // B) ÜYE KONTROLÜ: Veritabanından kontrol et
                else
                {
                    // Veritabanında bu email ve şifreye sahip kullanıcı var mı?
                    var user = _context.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

                    if (user != null)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Email), // E-posta veya Ad Soyad
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim(ClaimTypes.Role, user.Role) // Genelde "Member"
                        };
                        claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        isAuthenticated = true;
                    }
                }

                // C) GİRİŞ BAŞARILIYSA COOKIE OLUŞTUR
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

                    // Rolüne göre yönlendirme yap
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

        // Kayıt sayfası
        public IActionResult Register()
        {
            return View();
        }

        // Kayıt işlemi (GERÇEK VERİTABANI KAYDI)
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            // 1. ADIM: Checkbox kontrolünü MANUEL yapıyoruz.
            // [Range] attribute'ü sildiğimiz için, burada false ise hata fırlatıyoruz.
            if (model.AcceptTerms == false)
            {
                ModelState.AddModelError("AcceptTerms", "Kullanım şartlarını kabul etmelisiniz.");
            }

            // 2. ADIM: Diğer tüm kurallar (Boş alan, şifre uyumu vb.) uygun mu?
            if (ModelState.IsValid)
            {
                // 3. ADIM: Bu e-posta adresi daha önce kullanılmış mı?
                if (_context.Users.Any(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("", "Bu e-posta adresi zaten kayıtlı.");
                    return View(model);
                }

                // 4. ADIM: ViewModel'deki verileri gerçek User nesnesine aktar
                var newUser = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = model.Password, // Ödev olduğu için şifrelemeden (hashlemeden) kaydediyoruz
                    Role = "Member" // Varsayılan olarak "Üye" rolü veriyoruz

                    // DİKKAT: Eğer User.cs modelinde 'Phone' alanı tanımlı değilse aşağıdaki satırı silmelisin.
                    // Ama RegisterViewModel'de Phone olduğu için User tablosuna da eklemiş olmalısın.
                    // Phone = model.Phone 
                };

                // 5. ADIM: Veritabanına kaydet
                _context.Users.Add(newUser);
                _context.SaveChanges();

                // 6. ADIM: Başarılı mesajı ver ve Giriş sayfasına yönlendir
                TempData["Success"] = "Kayıt başarılı! Lütfen giriş yapınız.";
                return RedirectToAction("Login");
            }

            // Hata varsa formu verilerle birlikte tekrar göster
            return View(model);
        }

        // Çıkış işlemi
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}