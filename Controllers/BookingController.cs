// Controllers/BookingController.cs
using Microsoft.AspNetCore.Mvc;
using FitnessCenter.Models; // Models klasörünü kullanmak için

namespace FitnessCenter.Controllers
{
    public class BookingController : Controller
    {
        // Randevu alma sayfası
        public IActionResult Index()
        {
            return View();
        }

        // Randevu oluşturma
        [HttpPost]
        public IActionResult Create(BookingViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Veritabanına kaydetme simülasyonu...

                TempData["Success"] = "Randevunuz başarıyla oluşturuldu! Onay için e-posta gönderilecektir.";
                return RedirectToAction("Index", "Home");
            }

            return View("Index", model);
        }

        // Randevularım sayfası (üye paneli)
        public IActionResult MyBookings()
        {
            return View();
        }

        // Randevu iptal etme
        [HttpPost]
        public IActionResult Cancel(int id)
        {
            TempData["Success"] = "Randevunuz başarıyla iptal edildi.";
            return RedirectToAction("MyBookings");
        }
    }
}