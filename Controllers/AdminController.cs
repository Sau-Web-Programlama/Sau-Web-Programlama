using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SporSalonu2.Models;
using SporSalonu2.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System;

namespace SporSalonu2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index() { return View(); }

        public async Task<IActionResult> Trainers()
        {
            var trainers = await _context.Trainers.ToListAsync();
            return View(trainers);
        }

        [HttpGet]
        public async Task<IActionResult> CreateTrainer()
        {
            var model = new TrainerViewModel();
            model.AvailableServices = await _context.Services
                .Select(s => new SelectListItemDto
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToListAsync();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTrainer(TrainerViewModel model)
        {
            if (model.SelectedServiceIds == null || !model.SelectedServiceIds.Any())
            {
                ModelState.AddModelError("SelectedServiceIds", "En az bir uzmanlık alanı seçmelisiniz.");
            }

            if (ModelState.IsValid)
            {
                var selectedServiceNames = await _context.Services
                    .Where(s => model.SelectedServiceIds.Contains(s.Id))
                    .Select(s => s.Name)
                    .ToListAsync();

                string specialtyString = string.Join(", ", selectedServiceNames);

                var trainer = new Trainer
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Specialty = specialtyString,
                    Email = model.Email,
                    Phone = model.Phone,
                    Bio = model.Bio
                };

                _context.Trainers.Add(trainer);
                await _context.SaveChangesAsync();

                TempData["Success"] = $"{trainer.FirstName} {trainer.LastName} başarıyla eklendi!";
                return RedirectToAction("Trainers");
            }

            model.AvailableServices = await _context.Services
                .Select(s => new SelectListItemDto { Id = s.Id, Name = s.Name }).ToListAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditTrainer(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer == null)
            {
                TempData["Error"] = "Düzenlenecek antrenör bulunamadı.";
                return RedirectToAction("Trainers");
            }

            var model = new TrainerViewModel
            {
                Id = trainer.Id,
                FirstName = trainer.FirstName,
                LastName = trainer.LastName,
                Email = trainer.Email,
                Phone = trainer.Phone,
                Bio = trainer.Bio
            };

            var allServices = await _context.Services.ToListAsync();

            model.AvailableServices = allServices.Select(s => new SelectListItemDto
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();

            if (!string.IsNullOrEmpty(trainer.Specialty))
            {
                var currentSpecialties = trainer.Specialty.Split(',').Select(s => s.Trim()).ToList();
                model.SelectedServiceIds = allServices
                    .Where(s => currentSpecialties.Contains(s.Name))
                    .Select(s => s.Id)
                    .ToList();
            }

            ViewBag.TrainerId = id;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTrainer(int id, TrainerViewModel model)
        {
            if (model.SelectedServiceIds == null || !model.SelectedServiceIds.Any())
            {
                ModelState.AddModelError("SelectedServiceIds", "En az bir uzmanlık alanı seçmelisiniz.");
            }

            if (ModelState.IsValid)
            {
                var trainer = await _context.Trainers.FindAsync(id);
                if (trainer == null) return NotFound();

                var selectedServiceNames = await _context.Services
                    .Where(s => model.SelectedServiceIds.Contains(s.Id))
                    .Select(s => s.Name)
                    .ToListAsync();

                trainer.FirstName = model.FirstName;
                trainer.LastName = model.LastName;
                trainer.Specialty = string.Join(", ", selectedServiceNames);
                trainer.Email = model.Email;
                trainer.Phone = model.Phone;
                trainer.Bio = model.Bio;

                _context.Trainers.Update(trainer);
                await _context.SaveChangesAsync();

                TempData["Success"] = $"{trainer.FirstName} {trainer.LastName} başarıyla güncellendi!";
                return RedirectToAction("Trainers");
            }

            model.AvailableServices = await _context.Services
                .Select(s => new SelectListItemDto { Id = s.Id, Name = s.Name }).ToListAsync();

            ViewBag.TrainerId = id;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTrainer(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);

            if (trainer == null)
            {
                TempData["Error"] = "Silinecek antrenör bulunamadı.";
                return RedirectToAction("Trainers");
            }

            try
            {
                var availabilities = _context.Availabilities.Where(a => a.TrainerId == id);
                _context.Availabilities.RemoveRange(availabilities);

                var bookings = _context.Bookings.Where(b => b.TrainerId == id);
                _context.Bookings.RemoveRange(bookings);

                _context.Trainers.Remove(trainer);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Antrenör ve ilişkili tüm verileri başarıyla silindi.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Hata oluştu: " + ex.Message;
            }

            return RedirectToAction("Trainers");
        }

        public async Task<IActionResult> Services()
        {
            var services = await _context.Services.ToListAsync();
            return View(services);
        }

        [HttpGet]
        public IActionResult CreateService()
        {
            return View(new ServiceViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateService(ServiceViewModel model)
        {
            if (ModelState.IsValid)
            {
                var service = new Service
                {
                    Name = model.Name,
                    Description = model.Description,
                    DurationMinutes = model.DurationMinutes,
                    Price = model.Price
                };
                _context.Services.Add(service);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"'{service.Name}' hizmeti başarıyla eklendi!";
                return RedirectToAction("Services");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                TempData["Error"] = "Hizmet bulunamadı.";
                return RedirectToAction("Services");
            }
            var model = new ServiceViewModel
            {
                Name = service.Name,
                Description = service.Description,
                DurationMinutes = service.DurationMinutes,
                Price = service.Price
            };
            ViewBag.ServiceId = id;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditService(int id, ServiceViewModel model)
        {
            if (ModelState.IsValid)
            {
                var service = await _context.Services.FindAsync(id);
                if (service == null) return NotFound();

                service.Name = model.Name;
                service.Description = model.Description;
                service.DurationMinutes = model.DurationMinutes;
                service.Price = model.Price;

                _context.Services.Update(service);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"'{service.Name}' hizmeti güncellendi!";
                return RedirectToAction("Services");
            }
            ViewBag.ServiceId = id;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service != null)
            {
                _context.Services.Remove(service);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Hizmet silindi.";
            }
            return RedirectToAction("Services");
        }

        public async Task<IActionResult> Bookings()
        {
            var bookings = await _context.Bookings
                .Include(b => b.Trainer)
                .Include(b => b.Service)
                .OrderBy(b => b.Status != BookingStatus.Pending)
                .ThenByDescending(b => b.AppointmentDate)
                .ToListAsync();

            return View(bookings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                booking.Status = BookingStatus.Approved;
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Randevu (#{id}) onaylandı.";
            }
            else
            {
                TempData["Error"] = "Randevu bulunamadı.";
            }
            return RedirectToAction("Bookings");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                booking.Status = BookingStatus.Rejected;
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Randevu (#{id}) reddedildi.";
            }
            return RedirectToAction("Bookings");
        }

        public async Task<IActionResult> Members()
        {
            var members = await _context.Users
                                    .Where(u => u.Role == "Member")
                                    .ToListAsync();
            return View(members);
        }

        public IActionResult Reports() { return View(); }
    }
}