using Microsoft.AspNetCore.Mvc;

namespace FitnessCenter.Controllers
{
    public class HomeController : Controller
    {
        // Ana sayfa
        public IActionResult Index()
        {
            return View();
        }

        // Hakkýmýzda sayfasý (opsiyonel)
        public IActionResult About()
        {
            return View();
        }

        // Ýletiþim sayfasý (opsiyonel)
        public IActionResult Contact()
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