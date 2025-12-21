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
                  List<Slider> sliders = _db.Sliders.ToList();
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

                  var categoryIds = product.Categories
                  .Select(c => c.Id)
                  .ToList();

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