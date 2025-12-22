using Microsoft.AspNetCore.Mvc;

namespace ProniaFrontToBack.Areas.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        #region Index
        public IActionResult Index()
        {
            return View();
        }
        #endregion
    }
}
