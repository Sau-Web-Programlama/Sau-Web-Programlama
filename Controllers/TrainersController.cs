using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporSalonu2.Data;
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

        // Antrenörleri Listele (GET: Trainers)
        // Menüden "Antrenörler" butonuna basınca burası çalışır.
        public async Task<IActionResult> Index()
        {
            var trainers = await _context.Trainers.ToListAsync();
            return View(trainers);
        }
    }
}