using Microsoft.AspNetCore.Mvc;
using ProniaFrontToBack.DAL;
using ProniaFrontToBack.Models;
using ProniaFrontToBack.Utilities.ImageUpload;

namespace ProniaFrontToBack.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            _db = context;
            _env = env;
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
            if (!slider.ImageFile.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("ImageFile", "File type must be image");
                return View();
            }
            if (slider.ImageFile.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("ImageFile", "Image size must be max 2MB");
                return View();
            }
            var fileName = slider.ImageFile.SaveImage(_env, "Uploads/Slider");
            slider.Image = fileName;
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
            slider.Image?.DeleteImage(_env, "Uploads/Slider");
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