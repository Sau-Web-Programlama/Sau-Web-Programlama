using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SporSalonu2.Models;
using SporSalonu2.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace SporSalonu2.Controllers
{
    // Yalnızca "Admin" rolüne sahip kullanıcılar bu controller'daki sayfalara erişebilir.
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ------------------------------------
        // ANA SAYFA VE İSTATİSTİKLER (Index)
        // ------------------------------------
        public IActionResult Index() { return View(); }


        // ------------------------------------
        // ANTRENÖR YÖNETİMİ (CRUD - TAM)
        // ------------------------------------

        // Antrenörleri listeleme (READ)
        public async Task<IActionResult> Trainers()
        {
            var trainers = await _context.Trainers.ToListAsync();
            return View(trainers);
        }

        // Yeni Antrenör Ekleme Formunu Gösterme (GET - CREATE)
        [HttpGet]
        public IActionResult CreateTrainer() { return View(new TrainerViewModel()); }

        // Yeni Antrenör Ekleme İşlemi (POST - CREATE)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTrainer(TrainerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var trainer = new Trainer
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Specialty = model.Specialty,
                    Email = model.Email,
                    Phone = model.Phone,
                    Bio = model.Bio
                };

                _context.Trainers.Add(trainer);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"{trainer.FirstName} {trainer.LastName} başarıyla eklendi!";
                return RedirectToAction("Trainers");
            }
            return View(model);
        }

        // Antrenör Düzenleme Formunu Gösterme (GET - UPDATE)
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
                FirstName = trainer.FirstName,
                LastName = trainer.LastName,
                Specialty = trainer.Specialty,
                Email = trainer.Email,
                Phone = trainer.Phone,
                Bio = trainer.Bio
            };
            ViewBag.TrainerId = id;
            return View(model);
        }

        // Antrenör Düzenleme İşlemi (POST - UPDATE)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTrainer(int id, TrainerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var trainer = await _context.Trainers.FindAsync(id);
                if (trainer == null) return NotFound();

                trainer.FirstName = model.FirstName;
                trainer.LastName = model.LastName;
                trainer.Specialty = model.Specialty;
                trainer.Email = model.Email;
                trainer.Phone = model.Phone;
                trainer.Bio = model.Bio;

                _context.Trainers.Update(trainer);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"{trainer.FirstName} {trainer.LastName} başarıyla güncellendi!";
                return RedirectToAction("Trainers");
            }
            ViewBag.TrainerId = id;
            return View(model);
        }

        // Antrenör Silme İşlemi (POST - DELETE)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTrainer(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer != null)
            {
                _context.Trainers.Remove(trainer);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Antrenör başarıyla silindi.";
            }
            return RedirectToAction("Trainers");
        }


        // ------------------------------------
        // HİZMET YÖNETİMİ (CRUD - TAM)
        // ------------------------------------

        // Hizmetleri listeleme (READ)
        public async Task<IActionResult> Services()
        {
            var services = await _context.Services.ToListAsync();
            return View(services);
        }

        // Yeni Hizmet Ekleme Formunu Gösterme (GET - CREATE)
        [HttpGet]
        public IActionResult CreateService()
        {
            return View(new ServiceViewModel());
        }

        // Yeni Hizmet Ekleme İşlemi (POST - CREATE)
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

        // Hizmet Düzenleme Formunu Gösterme (GET - UPDATE)
        [HttpGet]
        public async Task<IActionResult> EditService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                TempData["Error"] = "Düzenlenecek hizmet bulunamadı.";
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

        // Hizmet Düzenleme İşlemi (POST - UPDATE)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditService(int id, ServiceViewModel model)
        {
            if (ModelState.IsValid)
            {
                var service = await _context.Services.FindAsync(id);
                if (service == null) return NotFound();

                service.Name = model.Name; service.Description = model.Description;
                service.DurationMinutes = model.DurationMinutes; service.Price = model.Price;

                _context.Services.Update(service);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"'{service.Name}' hizmeti başarıyla güncellendi!";
                return RedirectToAction("Services");
            }
            ViewBag.ServiceId = id;
            return View(model);
        }

        // Hizmet Silme İşlemi (POST - DELETE)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service != null)
            {
                _context.Services.Remove(service);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Hizmet başarıyla silindi.";
            }
            return RedirectToAction("Services");
        }


        // ------------------------------------
        // RANDEVU YÖNETİMİ (READ / ONAY MEKANİZMASI)
        // ------------------------------------

        // Randevuları listeleme (READ)
        public async Task<IActionResult> Bookings()
        {
            // İlişkili Trainer ve Service verilerini çekiyoruz
            var bookings = await _context.Bookings
                                 .Include(b => b.Trainer)
                                 .Include(b => b.Service)
                                 .OrderByDescending(b => b.AppointmentDate)
                                 .ToListAsync();

            return View(bookings);
        }

        // Randevu Onaylama (UPDATE)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                booking.Status = BookingStatus.Approved;
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Randevu #{id} onaylandı!";
            }
            return RedirectToAction("Bookings");
        }

        // Randevu Reddetme (UPDATE)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                booking.Status = BookingStatus.Rejected;
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Randevu #{id} reddedildi!";
            }
            return RedirectToAction("Bookings");
        }

        // ------------------------------------
        // DİĞER ADMIN SAYFALARI (İskeletler)
        // ------------------------------------
        public IActionResult Members() { return View(); }
        public IActionResult Reports() { return View(); }
    }
}