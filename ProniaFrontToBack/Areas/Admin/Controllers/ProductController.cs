using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaFrontToBack.Areas.Admin.ViewModels.Product;
using ProniaFrontToBack.DAL;
using ProniaFrontToBack.Models;

namespace ProniaFrontToBack.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        AppDbContext _db;
        public ProductController(AppDbContext db)
        {
            _db = db;
        }
        #region Index
        public IActionResult Index()
        {
            List<Product> products = _db.Products
            .Include(c => c.Categories)
            .Include(t => t.Tags)
            .ToList();
            return View(products);
        }
        #endregion

        #region Create (GET)
        public IActionResult Create()
        {
            ViewBag.Categories = _db.Categories.ToList();
            ViewBag.Tags = _db.Tags.ToList();
            return View();
        }
        #endregion

        #region Create (POST)
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM productVM)
        {
            ViewBag.Categories = _db.Categories.ToList();
            ViewBag.Tags = _db.Tags.ToList();
            if (!ModelState.IsValid) return View();
            if (productVM is null) return View("Error");

            Product product = new Product
            {
                Name = productVM.Name,
                Description = productVM.Description,
                Price = productVM.Price,
                Rating = productVM.Rating,
                SKU = productVM.SKU,
            };
            if (productVM.CategoryIds != null)
            {
                product.Categories = _db.Categories
                    .Where(c => productVM.CategoryIds.Contains(c.Id))
                    .ToList();
            }
            if (productVM.TagIds != null)
            {
                product.Tags = _db.Tags
                    .Where(t => productVM.TagIds.Contains(t.Id))
                    .ToList();
            }
            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return View("Error");
            Product? product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
            product.IsDeleted = true;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Restore
        [HttpPost]
        public async Task<IActionResult> Restore(int? id)
        {
            if (id is null) return View("Error");
            Product? product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
            product.IsDeleted = false;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Update (GET)
        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.Categories = await _db.Categories.ToListAsync();
            ViewBag.Tags = await _db.Tags.ToListAsync();

            Product? product = await _db.Products
                .Include(c => c.Categories)
                .Include(t => t.Tags)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product is null) return View("Error");

            UpdateProductVM productVM = new UpdateProductVM
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryIds = product.Categories.Select(c => c.Id).ToList(),
                TagIds = product.Tags.Select(t => t.Id).ToList()
            };
            return View(productVM);
        }
        #endregion

        #region Update (POST)
        [HttpPost]
        public async Task<IActionResult> Update(UpdateProductVM productVM)
        {
            ViewBag.Categories = await _db.Categories.ToListAsync();
            ViewBag.Tags = await _db.Tags.ToListAsync();

            if (!ModelState.IsValid) return View();

            Product? existProduct = await _db.Products
               .Include(c => c.Categories)
               .Include(t => t.Tags)
               .FirstOrDefaultAsync(p => p.Id == productVM.Id);

            if (existProduct is null) return View("Error");

            existProduct.Name = productVM.Name;
            existProduct.Description = productVM.Description;
            existProduct.Price = productVM.Price;
            existProduct.Categories.Clear();
            if (productVM.CategoryIds != null)
            {
                existProduct.Categories = _db.Categories
                    .Where(c => productVM.CategoryIds.Contains(c.Id))
                    .ToList();
            }
            existProduct.Tags.Clear();
            if (productVM.TagIds != null)
            {
                existProduct.Tags = _db.Tags
                    .Where(t => productVM.TagIds.Contains(t.Id))
                    .ToList();
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        #endregion

    }
}