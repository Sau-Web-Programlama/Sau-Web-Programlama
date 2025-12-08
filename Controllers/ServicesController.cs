// Controllers/ServicesController.cs
using Microsoft.AspNetCore.Mvc;

namespace FitnessCenter.Controllers
{
    public class ServicesController : Controller
    {
        // Hizmetler listesi
        public IActionResult Index()
        {
            return View();
        }

        // Hizmet detaylarý (opsiyonel - ileride eklenebilir)
        public IActionResult Details(int id)
        {
            // Veritabanýndan hizmet detaylarý çekilecek
            return View();
        }
    }
}