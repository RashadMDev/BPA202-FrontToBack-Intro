using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaFrontToBack.DAL;
using ProniaFrontToBack.Models;
using ProniaFrontToBack.ViewModels;

namespace ProniaFrontToBack.Controllers
{
      public class HomeController : Controller
      {
            AppDbContext _db;
            public HomeController(AppDbContext context)
            {
                  _db = context;
            }

            public IActionResult Index()
            {

                  Slider slider1 = new Slider
                  {
                        Id = 1,
                        Name = "Spring Collection",
                        Image = "1-1-524x617.png",
                        Description = "Discover our new spring collection with vibrant colors and fresh styles.",
                        DiscountRate = 20
                  };

                  Slider slider2 = new Slider
                  {
                        Id = 2,
                        Name = "Summer Sale",
                        Image = "1-4-770x300.jpg",
                        Description = "Enjoy the summer vibes with our exclusive summer sale offers.",
                        DiscountRate = 30
                  };

                  Slider slider3 = new Slider
                  {
                        Id = 3,
                        Name = "Autumn Arrivals",
                        Image = "1-1-1820x443.jpg",
                        Description = "Check out the latest autumn arrivals to refresh your wardrobe.",
                        DiscountRate = 25
                  };

                  List<Slider> sliders = new List<Slider>
                  {
                        slider1,
                        slider2,
                        slider3
                  };

                  List<Product> products = _db.Products
                  .Include(p => p.Images)
                  .ToList();

                  HomeVM homeVM = new HomeVM
                  {
                        Products = products,
                        Sliders = sliders
                  };

                  return View(homeVM);
            }
            public IActionResult Detail(int id)
            {
                  var product = _db.Products
                  .Include(p => p.Images)
                  .Include(p => p.Reviews)
                  .Include(p => p.Categories)
                  .Include(p => p.Tags)
                  .FirstOrDefault(p => p.Id == id);

                  var categoryIds = product.Categories.Select(c => c.Id).ToList();

                  var relatedProducts = _db.Products
                      .Include(p => p.Images)
                      .Include(p => p.Categories)
                      .Where(p => p.Categories.Any(c => categoryIds.Contains(c.Id)) && p.Id != product.Id)
                      .Distinct()
                      .Take(2)
                      .ToList();

                  DetailVM detailVM = new DetailVM
                  {
                        Product = product,
                        Products = relatedProducts
                  };

                  return View(detailVM);
            }
      }
}