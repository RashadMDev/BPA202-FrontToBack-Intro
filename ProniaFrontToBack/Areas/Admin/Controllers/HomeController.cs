using Microsoft.AspNetCore.Mvc;

namespace ProniaFrontToBack.Areas.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
