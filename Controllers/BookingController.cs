// Controllers/BookingController.cs
using Microsoft.AspNetCore.Mvc;

namespace FitnessCenter.Controllers
{
    public class BookingController : Controller
    {
        // Randevu alma sayfasý
        public IActionResult Index()
        {
            return View();
        }

        // Randevu oluþturma
        [HttpPost]
        public IActionResult Create(BookingViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Veritabanýna kaydet
                // Randevu çakýþmasý kontrolü yap
                // Onay maili gönder

                TempData["Success"] = "Randevunuz baþarýyla oluþturuldu! Onay için e-posta gönderilecektir.";
                return RedirectToAction("Index", "Home");
            }

            return View("Index", model);
        }

        // Randevularým sayfasý (üye paneli)
        public IActionResult MyBookings()
        {
            // Kullanýcýnýn randevularýný listele
            return View();
        }

        // Randevu iptal etme
        [HttpPost]
        public IActionResult Cancel(int id)
        {
            // Randevuyu iptal et
            TempData["Success"] = "Randevunuz baþarýyla iptal edildi.";
            return RedirectToAction("MyBookings");
        }
    }

    // ViewModel (þimdilik basit tutalým, Models klasöründe olacak)
    public class BookingViewModel
    {
        public string ServiceType { get; set; }
        public int TrainerId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
        public string Notes { get; set; }
    }
}