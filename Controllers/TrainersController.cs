// Controllers/TrainersController.cs
using Microsoft.AspNetCore.Mvc;

namespace FitnessCenter.Controllers
{
    public class TrainersController : Controller
    {
        // Antrenörler listesi
        public IActionResult Index()
        {
            return View();
        }

        // Antrenör detayları (opsiyonel - ileride eklenebilir)
        public IActionResult Details(int id)
        {
            // Veritabanından antrenör detayları çekilecek
            return View();
        }

        // Antrenöre göre müsait saatleri getir (AJAX için)
        [HttpGet]
        public IActionResult GetAvailableSlots(int trainerId, string date)
        {
            // Veritabanından müsait saatleri çek
            var availableSlots = new List<string>
            {
                "09:00", "10:00", "11:00", "14:00", "15:00", "16:00"
            };

            return Json(availableSlots);
        }
    }
}