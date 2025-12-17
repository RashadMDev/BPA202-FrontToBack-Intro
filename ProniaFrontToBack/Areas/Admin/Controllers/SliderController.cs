using Microsoft.AspNetCore.Mvc;
using ProniaFrontToBack.DAL;
using ProniaFrontToBack.Models;

namespace ProniaFrontToBack.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        AppDbContext _db;
        public SliderController(AppDbContext context)
        {
            _db = context;
        }
        public IActionResult Index()
        {
            List<Slider> sliders = _db.Sliders.ToList();
            return View(sliders);
        }
    }
}