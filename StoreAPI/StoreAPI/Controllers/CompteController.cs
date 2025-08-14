using Microsoft.AspNetCore.Mvc;

namespace StoreAPI.Controllers
{
    public class CompteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
