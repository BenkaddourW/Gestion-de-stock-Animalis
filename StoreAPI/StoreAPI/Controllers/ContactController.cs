using Microsoft.AspNetCore.Mvc;

namespace StoreAPI.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
