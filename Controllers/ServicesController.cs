using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporSalonu2.Data;
using System.Threading.Tasks;

namespace SporSalonu2.Controllers
{
    public class ServicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Constructor: Veritabanı bağlantısını (Dependency Injection ile) alıyoruz
        public ServicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Hizmetler listesi (GET: Services)
        public async Task<IActionResult> Index()
        {
            // Veritabanındaki 'Services' tablosundaki tüm verileri listeye çevirip View'a yolluyoruz.
            // Bu sayede Index.cshtml sayfasında @foreach ile dönebiliriz.
            var services = await _context.Services.ToListAsync();
            return View(services);
        }
    }
}