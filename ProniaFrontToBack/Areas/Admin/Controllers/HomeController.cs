namespace ProniaFrontToBack.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, SuperAdmin")]
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
