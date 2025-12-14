// Controllers/ServicesController.cs
using Microsoft.AspNetCore.Mvc;

namespace SporSalonu2.Controllers
{
    public class ServicesController : Controller
    {
        // Hizmetler listesi
        public IActionResult Index()
        {
            return View();
        }

        // Hizmet detayları (opsiyonel - ileride eklenebilir)
        public IActionResult Details(int id)
        {
            // Veritabanından hizmet detayları çekilecek
            return View();
        }
    }
}