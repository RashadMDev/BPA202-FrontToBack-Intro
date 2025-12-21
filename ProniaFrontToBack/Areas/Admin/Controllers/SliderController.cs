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
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Slider slider)
        {
            if (!ModelState.IsValid) return View();
            if (slider == null) return NotFound();
            _db.Sliders.Add(slider);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            Slider slider = _db.Sliders.FirstOrDefault(item => item.Id == id);
            if (slider == null) return NotFound();
            _db.Sliders.Remove(slider);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Update(int? id)
        {
            if (id == null) return NotFound();
            Slider slider = _db.Sliders.FirstOrDefault(item => item.Id == id);
            if (slider == null) return NotFound();
            return View(slider);
        }
        [HttpPost]
        public IActionResult Update(Slider newSlider)
        {
            if (!ModelState.IsValid) return View();
            if (newSlider == null) return NotFound();
            Slider oldSlider = _db.Sliders.FirstOrDefault(item => item.Id == newSlider.Id);
            if (oldSlider == null) return NotFound();

            oldSlider.Name = newSlider.Name;
            oldSlider.Description = newSlider.Description;
            oldSlider.Image = newSlider.Image;
            oldSlider.DiscountRate = newSlider.DiscountRate;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}