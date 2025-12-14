using Microsoft.AspNetCore.Mvc;
using SporSalonu2.Data;
using SporSalonu2.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SporSalonu2.Controllers
{
    // Bu attributelar, API'nin rotasını (api/Api) ve davranışını belirler
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // -------------------------------------------------------------
        // RANDEVU RAPORLAMA ENDPOINT'İ (Filtrelemeli LINQ Sorgusu)
        // Rota: GET /api/api/bookings/report?trainerId=1&startDate=2025-01-01
        // -------------------------------------------------------------
        [HttpGet("bookings/report")]
        public async Task<ActionResult<IEnumerable<BookingReportDto>>> GetBookingsReport(
            [FromQuery] int? trainerId,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            // IQueryable oluşturma ve ilişkili verileri (Trainer, Service) dahil etme
            var query = _context.Bookings
                                 .Include(b => b.Trainer)
                                 .Include(b => b.Service)
                                 .AsQueryable();

            // LINQ Filtreleme Mantığı:

            // 1. Antrenör ID'sine göre filtreleme
            if (trainerId.HasValue)
            {
                query = query.Where(b => b.TrainerId == trainerId.Value);
            }

            // 2. Başlangıç tarihine göre filtreleme
            if (startDate.HasValue)
            {
                query = query.Where(b => b.AppointmentDate.Date >= startDate.Value.Date);
            }

            // 3. Bitiş tarihine göre filtreleme
            if (endDate.HasValue)
            {
                query = query.Where(b => b.AppointmentDate.Date <= endDate.Value.Date);
            }

            // 4. Verileri çekme ve DTO'ya dönüştürme (Projection)
            var reportData = await query
                .OrderBy(b => b.AppointmentDate)
                .Select(b => new BookingReportDto
                {
                    Id = b.Id,
                    MemberEmail = b.MemberId,
                    TrainerName = b.Trainer.FirstName + " " + b.Trainer.LastName,
                    ServiceName = b.Service.Name,
                    AppointmentDate = b.AppointmentDate,
                    AppointmentTime = b.AppointmentTime,
                    Status = b.Status.ToString() // Enum'ı string metne çevirir
                })
                .ToListAsync();

            if (!reportData.Any())
            {
                // Filtrelemeye uygun randevu yoksa 404 döndür
                return NotFound("Filtrelere uygun randevu bulunamadı.");
            }

            // Veri varsa 200 OK ile DTO listesini döndür
            return Ok(reportData);
        }

        // -------------------------------------------------------------
        // YARDIMCI ENDPOINT: TÜM ANTRENÖRLERİ DÖNDÜR
        // -------------------------------------------------------------
        [HttpGet("trainers")]
        public async Task<ActionResult<IEnumerable<object>>> GetTrainers()
        {
            // Raporlama sayfasındaki filtreleme dropdown'u için Antrenör listesini döndürür.
            var trainers = await _context.Trainers
                .Select(t => new { t.Id, FullName = t.FirstName + " " + t.LastName })
                .ToListAsync();

            return Ok(trainers);
        }
    }
}