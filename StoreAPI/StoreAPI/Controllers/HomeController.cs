using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using StoreAPI.Models;

namespace StoreAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // R�cup�rer le nom de l'utilisateur
            // 
            var userName = HttpContext.Session.GetString("UserName");

            // V�rifier si le nom est pr�sent
            // 
            ViewData["UserName"] = userName ?? "Invit�";

            return View();
        }
        public IActionResult Logout()
        {
            // Supprimer l'utilisateur de la session
            HttpContext.Session.Remove("UserName");

            // Rediriger vers la page de connexion
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
