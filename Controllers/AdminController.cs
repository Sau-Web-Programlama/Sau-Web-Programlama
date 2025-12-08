// Controllers/AdminController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessCenter.Controllers
{
    [Authorize(Roles = "Admin")] // Sadece Admin rolü eriþebilir
    public class AdminController : Controller
    {
        // Admin ana sayfa
        public IActionResult Index()
        {
            return View();
        }

        // Antrenörler yönetimi
        public IActionResult Trainers()
        {
            // Veritabanýndan antrenörleri çek
            return View();
        }

        // Yeni antrenör ekleme
        [HttpGet]
        public IActionResult CreateTrainer()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateTrainer(TrainerViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Veritabanýna kaydet
                TempData["Success"] = "Antrenör baþarýyla eklendi!";
                return RedirectToAction("Trainers");
            }
            return View(model);
        }

        // Hizmetler yönetimi
        public IActionResult Services()
        {
            // Veritabanýndan hizmetleri çek
            return View();
        }

        // Randevu yönetimi
        public IActionResult Bookings()
        {
            // Tüm randevularý listele
            return View();
        }

        // Randevu onaylama
        [HttpPost]
        public IActionResult ApproveBooking(int id)
        {
            // Randevuyu onayla
            TempData["Success"] = "Randevu onaylandý!";
            return RedirectToAction("Bookings");
        }

        // Randevu reddetme
        [HttpPost]
        public IActionResult RejectBooking(int id)
        {
            // Randevuyu reddet
            TempData["Success"] = "Randevu reddedildi!";
            return RedirectToAction("Bookings");
        }

        // Üye yönetimi
        public IActionResult Members()
        {
            // Tüm üyeleri listele
            return View();
        }

        // Raporlar
        public IActionResult Reports()
        {
            // Ýstatistikler ve raporlar
            return View();
        }
    }

    // Basit ViewModel (Models klasöründe olacak)
    public class TrainerViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Specialty { get; set; }
        public string Bio { get; set; }
    }
}