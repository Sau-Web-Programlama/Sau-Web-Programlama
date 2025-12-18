using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporSalonu2.Data;
using SporSalonu2.Models;
using System;
using System.Collections.Generic;
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

        public async Task<IActionResult> Index()
        {
            var trainers = await _context.Trainers.ToListAsync();
            return View(trainers);
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
    }
}