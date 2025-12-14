<<<<<<< Updated upstream
﻿// Controllers/BookingController.cs
using Microsoft.AspNetCore.Mvc;
using FitnessCenter.Models; // Models klasörünü kullanmak için
=======
﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporSalonu2.Data;
using SporSalonu2.Models;
using System.Security.Claims; // ClaimTypes için gerekli
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization; // [Authorize] için gerekli
using System.Linq; // LINQ metotları için gerekli
>>>>>>> Stashed changes

namespace SporSalonu2.Controllers
{
    public class BookingController : Controller
    {
<<<<<<< Updated upstream
        // Randevu alma sayfası
        public IActionResult Index()
=======
        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context)
>>>>>>> Stashed changes
        {
            _context = context;
        }

<<<<<<< Updated upstream
        // Randevu oluşturma
        [HttpPost]
        public IActionResult Create(BookingViewModel model)
=======
        // ------------------------------------
        // RANDEVU ALMA FORMU (GET)
        // ------------------------------------
        // Bu metot, formu doldurmak için gerekli olan Antrenör ve Hizmet listelerini hazırlar.
        public async Task<IActionResult> Index()
>>>>>>> Stashed changes
        {
            var viewModel = new BookingViewModel
            {
                // Antrenör listesini DTO'ya dönüştürerek yükleme
                Trainers = await _context.Trainers.Select(t => new SelectListItemDto
                {
                    Id = t.Id,
                    Name = t.FirstName + " " + t.LastName
                }).ToListAsync(),

                // Hizmet listesini DTO'ya dönüştürerek yükleme
                Services = await _context.Services.Select(s => new SelectListItemDto
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToListAsync()
            };

            return View(viewModel);
        }

        // ------------------------------------
        // RANDEVU OLUŞTURMA (POST - CREATE)
        // ------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookingViewModel model)
        {
            // HATA ÇÖZÜMÜ: ModelState.IsValid başarısız olursa, View'e geri dönerken listeleri tekrar yüklememiz gerekiyor.
            // Bu sefer anonim tipler yerine DTO kullanarak yükleme yapıyoruz:
            model.Trainers = await _context.Trainers.Select(t => new SelectListItemDto { Id = t.Id, Name = t.FirstName + " " + t.LastName }).ToListAsync();
            model.Services = await _context.Services.Select(s => new SelectListItemDto { Id = s.Id, Name = s.Name }).ToListAsync();

            if (ModelState.IsValid)
            {
<<<<<<< Updated upstream
                // Veritabanına kaydetme simülasyonu...

                TempData["Success"] = "Randevunuz başarıyla oluşturuldu! Onay için e-posta gönderilecektir.";
                return RedirectToAction("Index", "Home");
=======
                // Oturum açmış üyenin ID'sini (e-posta) alırız.
                var memberId = User.FindFirstValue(ClaimTypes.Email);

                if (string.IsNullOrEmpty(memberId))
                {
                    // Giriş yapılmamışsa, yönlendir
                    return RedirectToAction("Login", "Account");
                }

                // Yeni randevu entity'si oluşturma
                var booking = new Booking
                {
                    MemberId = memberId,
                    TrainerId = model.TrainerId,
                    ServiceId = model.ServiceType,
                    AppointmentDate = model.AppointmentDate.Date,
                    AppointmentTime = model.AppointmentTime,
                    Notes = model.Notes,
                    Status = BookingStatus.Pending // Randevu onaya düşmeli
                };

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Randevunuz başarıyla oluşturuldu! Onay için admin onayı bekleniyor.";
                return RedirectToAction("MyBookings");
>>>>>>> Stashed changes
            }

            // Doğrulama başarısız olursa, formu hatalarla birlikte tekrar göster
            return View("Index", model);
        }

<<<<<<< Updated upstream
        // Randevularım sayfası (üye paneli)
        public IActionResult MyBookings()
        {
            return View();
=======
        // ------------------------------------
        // ÜYE RANDEVULARIM (READ)
        // ------------------------------------
        [Authorize(Roles = "Member,Admin")] // Üye veya Admin yetkisine sahip kullanıcılar görebilmeli
        public async Task<IActionResult> MyBookings()
        {
            var memberId = User.FindFirstValue(ClaimTypes.Email);

            // Üyenin randevularını, ilişkili Antrenör ve Hizmet bilgileriyle çek.
            var myBookings = await _context.Bookings
                                   .Include(b => b.Trainer)
                                   .Include(b => b.Service)
                                   .Where(b => b.MemberId == memberId)
                                   .OrderByDescending(b => b.AppointmentDate)
                                   .ToListAsync();

            return View(myBookings);
>>>>>>> Stashed changes
        }

        // ------------------------------------
        // RANDEVU İPTALİ (UPDATE)
        // ------------------------------------
        [HttpPost]
        [Authorize(Roles = "Member,Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
<<<<<<< Updated upstream
            TempData["Success"] = "Randevunuz başarıyla iptal edildi.";
=======
            var booking = await _context.Bookings.FindAsync(id);
            var memberId = User.FindFirstValue(ClaimTypes.Email);

            // Randevunun varlığını ve randevuyu talep eden kişinin o anki kullanıcı olduğunu kontrol et
            if (booking == null || booking.MemberId != memberId)
            {
                TempData["Error"] = "Randevu bulunamadı veya iptal yetkiniz yok.";
                return RedirectToAction("MyBookings");
            }

            // Randevuyu iptal etme işlemi (fiziksel silme yerine durumu değiştirme)
            booking.Status = BookingStatus.Cancelled;
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Randevunuz (#{id}) başarıyla iptal edildi.";
>>>>>>> Stashed changes
            return RedirectToAction("MyBookings");
        }
    }
}