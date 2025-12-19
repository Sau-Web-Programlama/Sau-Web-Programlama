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

        // --- ANTRENÖR YÖNETİMİ ---
        public async Task<IActionResult> Trainers()
        {
            return View(await _context.Trainers.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> CreateTrainer()
        {
            var model = new TrainerViewModel();
            model.AvailableServices = await _context.Services.Select(s => new SelectListItemDto { Id = s.Id, Name = s.Name }).ToListAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // DÜZELTME: Metot adı CreateTrainer yapıldı ve selectedDays parametresi eklendi
        public async Task<IActionResult> CreateTrainer(TrainerViewModel model, List<string> selectedDays)
        {
            if (model.SelectedServiceIds == null || !model.SelectedServiceIds.Any()) ModelState.AddModelError("SelectedServiceIds", "Seçim yapmalısınız.");

            if (ModelState.IsValid)
            {
                var services = await _context.Services.Where(s => model.SelectedServiceIds.Contains(s.Id)).Select(s => s.Name).ToListAsync();

                var trainer = new Trainer
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Specialty = string.Join(", ", services),
                    Email = model.Email,
                    Phone = model.Phone,
                    Bio = model.Bio,

                    // Çalışma Günlerini Kaydetme
                    WorkingDays = selectedDays != null ? string.Join(",", selectedDays) : ""
                };

                _context.Trainers.Add(trainer);
                await _context.SaveChangesAsync();
                return RedirectToAction("Trainers");
            }

            model.AvailableServices = await _context.Services.Select(s => new SelectListItemDto { Id = s.Id, Name = s.Name }).ToListAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditTrainer(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer == null) return RedirectToAction("Trainers");

            var model = new TrainerViewModel
            {
                Id = trainer.Id,
                FirstName = trainer.FirstName,
                LastName = trainer.LastName,
                Email = trainer.Email,
                Phone = trainer.Phone,
                Bio = trainer.Bio
            };

            // Uzmanlık Alanlarını Doldurma (Eski kodun)
            var allServices = await _context.Services.ToListAsync();
            model.AvailableServices = allServices.Select(s => new SelectListItemDto { Id = s.Id, Name = s.Name }).ToList();

            if (!string.IsNullOrEmpty(trainer.Specialty))
            {
                var current = trainer.Specialty.Split(',').Select(s => s.Trim()).ToList();
                model.SelectedServiceIds = allServices.Where(s => current.Contains(s.Name)).Select(s => s.Id).ToList();
            }

            // --- YENİ EKLENEN KISIM: ÇALIŞMA GÜNLERİNİ GETİRME ---
            // Veritabanındaki string'i (Monday,Tuesday) listeye çevirip ViewBag'e atıyoruz.
            ViewBag.SelectedDays = !string.IsNullOrEmpty(trainer.WorkingDays)
                ? trainer.WorkingDays.Split(',').ToList()
                : new List<string>();
            // -----------------------------------------------------

            ViewBag.TrainerId = id;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // Edit işlemine de 'selectedDays' eklendi.
        public async Task<IActionResult> EditTrainer(int id, TrainerViewModel model, List<string> selectedDays)
        {
            if (ModelState.IsValid)
            {
                var trainer = await _context.Trainers.FindAsync(id);
                var services = await _context.Services.Where(s => model.SelectedServiceIds.Contains(s.Id)).Select(s => s.Name).ToListAsync();

                trainer.FirstName = model.FirstName;
                trainer.LastName = model.LastName;
                trainer.Specialty = string.Join(", ", services);
                trainer.Email = model.Email;
                trainer.Phone = model.Phone;
                trainer.Bio = model.Bio;

                if (selectedDays != null)
                {
                    trainer.WorkingDays = string.Join(",", selectedDays);
                }

                _context.Trainers.Update(trainer);
                await _context.SaveChangesAsync();
                return RedirectToAction("Trainers");
            }

            model.AvailableServices = await _context.Services.Select(s => new SelectListItemDto { Id = s.Id, Name = s.Name }).ToListAsync();
            ViewBag.TrainerId = id;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTrainer(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer != null)
            {
                _context.Bookings.RemoveRange(_context.Bookings.Where(b => b.TrainerId == id));
                _context.Trainers.Remove(trainer);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Trainers");
        }

        // --- HİZMETLER ---
        public async Task<IActionResult> Services() => View(await _context.Services.ToListAsync());

        [HttpGet] public IActionResult CreateService() => View(new ServiceViewModel());
        [HttpPost] public async Task<IActionResult> CreateService(ServiceViewModel m) { if (ModelState.IsValid) { _context.Services.Add(new Service { Name = m.Name, Description = m.Description, DurationMinutes = m.DurationMinutes, Price = m.Price }); await _context.SaveChangesAsync(); return RedirectToAction("Services"); } return View(m); }

        // --- HİZMET DÜZENLEME ---
        [HttpGet]
        public async Task<IActionResult> EditService(int id)
        {
            var s = await _context.Services.FindAsync(id);
            if (s == null) return RedirectToAction("Services");

            var model = new ServiceViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                DurationMinutes = s.DurationMinutes,
                Price = s.Price
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditService(ServiceViewModel model)
        {
            if (ModelState.IsValid)
            {
                var s = await _context.Services.FindAsync(model.Id);
                if (s == null) return NotFound();

                s.Name = model.Name;
                s.Description = model.Description;
                s.DurationMinutes = model.DurationMinutes;
                s.Price = model.Price;

                await _context.SaveChangesAsync();
                return RedirectToAction("Services");
            }
            return View(model);
        }
        [HttpPost] public async Task<IActionResult> DeleteService(int id) { var s = await _context.Services.FindAsync(id); if (s != null) { _context.Services.Remove(s); await _context.SaveChangesAsync(); } return RedirectToAction("Services"); }

        // --- RANDEVULAR ---
        public async Task<IActionResult> Bookings() => View(await _context.Bookings.Include(b => b.Trainer).Include(b => b.Service).OrderByDescending(b => b.AppointmentDate).ToListAsync());
        [HttpPost] public async Task<IActionResult> ApproveBooking(int id) { var b = await _context.Bookings.FindAsync(id); if (b != null) { b.Status = BookingStatus.Approved; await _context.SaveChangesAsync(); } return RedirectToAction("Bookings"); }
        [HttpPost] public async Task<IActionResult> RejectBooking(int id) { var b = await _context.Bookings.FindAsync(id); if (b != null) { b.Status = BookingStatus.Rejected; await _context.SaveChangesAsync(); } return RedirectToAction("Bookings"); }

        // --- ÜYE YÖNETİMİ ---
        public async Task<IActionResult> Members()
        {
            var members = await _context.Users.Where(u => u.Role == "Member").ToListAsync();
            return View(members);
        }

        [HttpGet]
        public async Task<IActionResult> EditMember(int id)
        {
            if (id <= 0) return NotFound();
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            var model = new MemberEditViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMember(MemberEditViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _context.Users.FindAsync(model.Id);
            if (user == null) return RedirectToAction("Members");

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.Phone = model.Phone;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Üye güncellendi.";
            return RedirectToAction("Members");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return RedirectToAction("Members");

            var bookings = _context.Bookings.Where(b => b.MemberId == user.Email);
            _context.Bookings.RemoveRange(bookings);

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Üye silindi.";
            return RedirectToAction("Members");
        }

        public IActionResult Reports() { return View(); }
    }
}