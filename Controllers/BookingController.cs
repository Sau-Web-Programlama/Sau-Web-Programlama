using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporSalonu2.Data;
using SporSalonu2.Models;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace SporSalonu2.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = new BookingViewModel
            {
                Services = await _context.Services
                    .Select(s => new SelectListItemDto { Id = s.Id, Name = s.Name })
                    .ToListAsync(),
                Trainers = new List<SelectListItemDto>()
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetTrainersByService(int serviceId)
        {
            var service = await _context.Services.FindAsync(serviceId);
            if (service == null) return Json(new List<object>());

            var trainers = await _context.Trainers
                .Where(t => t.Specialty.Contains(service.Name))
                .Select(t => new { id = t.Id, name = t.FirstName + " " + t.LastName })
                .ToListAsync();

            return Json(trainers);
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailableSlots(int trainerId, string dateStr)
        {
            if (trainerId == 0 || string.IsNullOrEmpty(dateStr)) return Json(new List<string>());

            if (!DateTime.TryParse(dateStr, out DateTime selectedDate))
                return Json(new List<string>());

            var allSlots = new List<string>();
            TimeSpan start = new TimeSpan(9, 0, 0);
            TimeSpan end = new TimeSpan(22, 0, 0);

            while (start < end)
            {
                allSlots.Add(start.ToString(@"hh\:mm"));
                start = start.Add(TimeSpan.FromHours(1));
            }

            var bookedSlots = await _context.Bookings
                .Where(b => b.TrainerId == trainerId
                         && b.AppointmentDate.Date == selectedDate.Date
                         && b.Status != BookingStatus.Rejected
                         && b.Status != BookingStatus.Cancelled)
                .Select(b => b.AppointmentTime)
                .ToListAsync();

            var availableSlots = allSlots.Except(bookedSlots).ToList();

            if (selectedDate.Date == DateTime.Today)
            {
                var now = DateTime.Now.TimeOfDay;
                availableSlots = availableSlots
                    .Where(slot => TimeSpan.Parse(slot) > now)
                    .ToList();
            }

            return Json(availableSlots);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookingViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();

                TempData["Error"] = "Form Hatası: " + string.Join(" | ", errors);

                model.Services = await _context.Services
                     .Select(s => new SelectListItemDto { Id = s.Id, Name = s.Name }).ToListAsync();
                model.Trainers = new List<SelectListItemDto>();

                return View("Index", model);
            }

            try
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email) ?? User.Identity.Name;

                var booking = new Booking
                {
                    MemberId = userEmail,
                    TrainerId = model.TrainerId,
                    ServiceId = model.ServiceType,
                    AppointmentDate = model.AppointmentDate,
                    AppointmentTime = model.AppointmentTime,
                    Notes = model.Notes,
                    Status = BookingStatus.Pending
                };

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Randevu başarıyla oluşturuldu!";
                return RedirectToAction("MyBookings");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Veritabanı Hatası: " + ex.Message;

                model.Services = await _context.Services
                     .Select(s => new SelectListItemDto { Id = s.Id, Name = s.Name }).ToListAsync();
                model.Trainers = new List<SelectListItemDto>();
                return View("Index", model);
            }
        }

        public async Task<IActionResult> MyBookings()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email) ?? User.Identity.Name;

            var list = await _context.Bookings
                .Include(b => b.Trainer)
                .Include(b => b.Service)
                .Where(b => b.MemberId == userEmail)
                .OrderByDescending(b => b.AppointmentDate)
                .ToListAsync();

            return View(list);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            var userEmail = User.FindFirstValue(ClaimTypes.Email) ?? User.Identity.Name;

            if (booking == null || booking.MemberId != userEmail)
            {
                TempData["Error"] = "İptal yetkiniz yok veya randevu bulunamadı.";
                return RedirectToAction("MyBookings");
            }

            if (booking.Status == BookingStatus.Cancelled || booking.Status == BookingStatus.Rejected)
            {
                TempData["Error"] = "Bu randevu zaten iptal edilmiş veya reddedilmiş.";
                return RedirectToAction("MyBookings");
            }

            booking.Status = BookingStatus.Cancelled;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Randevunuz iptal edildi.";
            return RedirectToAction("MyBookings");
        }
    }
}