<<<<<<< Updated upstream
﻿// Controllers/TrainersController.cs
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
// Controllers/TrainersController.cs
using Microsoft.AspNetCore.Mvc;
=======
﻿using Microsoft.AspNetCore.Mvc;
using SporSalonu2.Data; // DbContext için
using Microsoft.EntityFrameworkCore; // ToListAsync ve Include için
using System;
using System.Linq;
using System.Threading.Tasks;
>>>>>>> Stashed changes

namespace SporSalonu2.Controllers
{
    public class TrainersController : Controller
    {
<<<<<<< Updated upstream
        // Antrenörler listesi
        public IActionResult Index()
=======
        private readonly ApplicationDbContext _context;

        public TrainersController(ApplicationDbContext context)
>>>>>>> Stashed changes
        {
            _context = context;
        }

<<<<<<< Updated upstream
        // Antrenör detayları (opsiyonel - ileride eklenebilir)
        public IActionResult Details(int id)
        {
            // Veritabanından antrenör detayları çekilecek
            return View();
        }

        // Antrenöre göre müsait saatleri getir (AJAX için)
=======
        // ... Index ve Details Action'ları burada kalsın ...

        // Antrenöre ve tarihe göre müsait saatleri getir (AJAX için)
>>>>>>> Stashed changes
        [HttpGet]
        public async Task<IActionResult> GetAvailableSlots(int trainerId, string date)
        {
<<<<<<< Updated upstream
            // Veritabanından müsait saatleri çek
            var availableSlots = new List<string>
=======
            if (trainerId <= 0 || string.IsNullOrEmpty(date))
>>>>>>> Stashed changes
            {
                return Json(new List<string>());
            }

            // Seçilen tarihi System.DateTime objesine çevir
            if (!DateTime.TryParse(date, out DateTime selectedDate))
            {
                return Json(new List<string>());
            }

            // Seçilen günün WeekDay enum karşılığını bul
            var selectedDayOfWeek = (SporSalonu2.Models.DayOfWeek)((int)selectedDate.DayOfWeek == 0 ? 6 : (int)selectedDate.DayOfWeek - 1);

            // Veritabanından o gün ve antrenöre ait müsaitlik aralıklarını çek
            var availableTimes = await _context.Availabilities
                .Where(a => a.TrainerId == trainerId && a.DayOfWeek == selectedDayOfWeek)
                .SelectMany(a => GenerateSlots(a.StartTime, a.EndTime, 60)) // Her seansı 60 dk kabul ettik.
                .ToListAsync();

            // ÖNEMLİ: Daha önce alınan randevuları kontrol et
            var bookedTimes = await _context.Bookings
                .Where(b => b.TrainerId == trainerId && b.AppointmentDate.Date == selectedDate.Date && b.Status == SporSalonu2.Models.BookingStatus.Approved)
                .Select(b => b.AppointmentTime)
                .ToListAsync();

            // Sadece rezerve edilmemiş saatleri döndür
            var finalSlots = availableTimes.Except(bookedTimes).ToList();

            return Json(finalSlots);
        }

        public async Task<IActionResult> Index()
        {
            // Antrenörleri çekmek için ApplicationDbContext'i kullanıyoruz.
            var trainers = await _context.Trainers.ToListAsync();
            return View(trainers);
        }



        // Yardımcı metot: Başlangıç/Bitiş saat aralığından 60 dakikalık slotlar üretir.
        private IEnumerable<string> GenerateSlots(TimeSpan start, TimeSpan end, int durationMinutes)
        {
            var slots = new List<string>();
            var current = start;
            while (current.Add(TimeSpan.FromMinutes(durationMinutes)) <= end)
            {
                slots.Add(current.ToString(@"hh\:mm"));
                current = current.Add(TimeSpan.FromMinutes(durationMinutes));
            }
            return slots;
        }
    }
}