using ProniaFrontToBack.Utilities.ImageUpload;

namespace ProniaFrontToBack.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            _db = context;
            _env = env;
        }

        #region Index
        public IActionResult Index()
        {
            List<Slider> sliders = _db.Sliders
                .ToList();
            return View(sliders);
        }
        #endregion

        #region Create (GET)
        public IActionResult Create()
        {
            return View();
        }
        #endregion

        #region Create (POST)
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
            if (slider == null) return View("Error");
            _db.Sliders.Add(slider);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion

        #region Soft Delete
        [HttpPost]
        public IActionResult Delete(int? id)
        {
            if (id == null) return View("Error");
            Slider? slider = _db.Sliders.FirstOrDefault(item => item.Id == id);
            if (slider == null) return View("Error");
            slider.IsDeleted = true;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion

        #region Restore
        [HttpPost]
        public IActionResult Restore(int? id)
        {
            if (id == null) return View("Error");
            Slider? slider = _db.Sliders.FirstOrDefault(item => item.Id == id);
            if (slider == null) return View("Error");
            slider.IsDeleted = false;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion

        #region Update (GET)
        public IActionResult Update(int? id)
        {
            if (id == null) return View("Error");
            Slider? slider = _db.Sliders.FirstOrDefault(item => item.Id == id);
            if (slider == null) return View("Error");
            return View(slider);
        }
        #endregion

        #region Update (POST)
        [HttpPost]
        public IActionResult Update(Slider newSlider)
        {
            if (!ModelState.IsValid) return View();
            if (newSlider == null) return View("Error");
            Slider? oldSlider = _db.Sliders.FirstOrDefault(item => item.Id == newSlider.Id);
            if (oldSlider == null) return View("Error");

            oldSlider.Name = newSlider.Name;
            oldSlider.Description = newSlider.Description;
            oldSlider.Image = newSlider.Image;
            oldSlider.DiscountRate = newSlider.DiscountRate;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion
    }
}