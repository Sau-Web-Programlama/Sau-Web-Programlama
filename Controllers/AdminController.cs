// Controllers/AdminController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FitnessCenter.Models;

namespace FitnessCenter.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public IActionResult Index() { return View(); }
        public IActionResult Trainers() { return View(); }

        [HttpGet]
        public IActionResult CreateTrainer() { return View(); }

        [HttpPost]
        public IActionResult CreateTrainer(TrainerViewModel model)
        {
            if (ModelState.IsValid)
            {
                TempData["Success"] = "Antrenör başarıyla eklendi!";
                return RedirectToAction("Trainers");
            }
            return View(model);
        }

        public IActionResult Services() { return View(); }
        public IActionResult Bookings() { return View(); }
        public IActionResult Members() { return View(); }
        public IActionResult Reports() { return View(); }

        [HttpPost]
        public IActionResult ApproveBooking(int id)
        {
            TempData["Success"] = "Randevu onaylandı!";
            return RedirectToAction("Bookings");
        }

        [HttpPost]
        public IActionResult RejectBooking(int id)
        {
            TempData["Success"] = "Randevu reddedildi!";
            return RedirectToAction("Bookings");
        }
    }
}