using Microsoft.AspNetCore.Mvc;

namespace SporSalonu2.Controllers
{
    public class HomeController : Controller
    {
        // Ana sayfa
        public IActionResult Index()
        {
            return View();
        }

        // Hata sayfasý
        public IActionResult Error()
        {
            return View();
        }
    }
}