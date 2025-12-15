using Microsoft.AspNetCore.Mvc;
using SporSalonu2.Data; // DbContext için
using Microsoft.EntityFrameworkCore; // ToListAsync ve Include için
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SporSalonu2.Controllers
{
    public class TrainersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrainersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ... Index ve Details Action'ları burada kalsın ...

        // Antrenöre ve tarihe göre müsait saatleri getir (AJAX için)
        [HttpGet]
        public async Task<IActionResult> GetAvailableSlots(int trainerId, string date)
        {
            if (trainerId <= 0 || string.IsNullOrEmpty(date))
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