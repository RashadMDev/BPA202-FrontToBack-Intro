using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaFrontToBack.Areas.Admin.ViewModels.Product;
using ProniaFrontToBack.DAL;
using ProniaFrontToBack.Models;
using ProniaFrontToBack.Utilities.ImageUpload;

namespace ProniaFrontToBack.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        AppDbContext _db;
        IWebHostEnvironment _env;
        public ProductController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        #region Index
        public IActionResult Index()
        {
            List<Product> products = _db.Products
            .Include(p => p.Images)
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
                Images = new List<Image>()
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

            if (!productVM.PrimaryImage.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("PrimaryImage", "File type must be image");
                return View(productVM);
            }
            if (productVM.PrimaryImage.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("PrimaryImage", "File size must be less than 2MB");
                return View(productVM);
            }
            var primaryImageFileName = productVM.PrimaryImage.SaveImage(_env, "Uploads/Product");
            product.Images.Add(new Image()
            {
                Url = primaryImageFileName,
                IsPrimary = true
            });

            foreach (var image in productVM.Images)
            {
                if (!image.ContentType.Contains("image/"))
                {
                    ModelState.AddModelError("Images", "File type must be image");
                    return View(productVM);
                }
                if (image.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("Images", "File size must be less than 2MB");
                    return View(productVM);
                }
                var otherImageFileNames = image.SaveImage(_env, "Uploads/Product");
                product.Images.Add(new Image()
                {
                    Url = otherImageFileNames,
                });
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
                .Include(i => i.Images)
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
                TagIds = product.Tags.Select(t => t.Id).ToList(),
                OldImages = product.Images
                .Select(i => new ProductImagesVM
                {
                    ImgURL = i.Url,
                    IsPrimary = i.IsPrimary
                }).ToList()
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

            if (!ModelState.IsValid) return View(productVM);

            Product? existProduct = await _db.Products
                .Include(i => i.Images)
                .Include(c => c.Categories)
                .Include(t => t.Tags)
                .FirstOrDefaultAsync(p => p.Id == productVM.Id);

            if (existProduct is null) return View("Error");


            if (productVM.PrimaryImage is not null)
            {
                if (!productVM.PrimaryImage.ContentType.Contains("image/"))
                {
                    ModelState.AddModelError("PrimaryImage", "File type must be image");
                    return View(productVM);
                }
                if (productVM.PrimaryImage.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("PrimaryImage", "File size must be less than 2MB");
                    return View(productVM);
                }
                var primaryImage = existProduct.Images.FirstOrDefault(i => i.IsPrimary);
                if (primaryImage != null)
                {
                    ImageExtension.DeleteImage(primaryImage.Url, _env, "Uploads/Product");
                    _db.Images.Remove(primaryImage);
                }
                existProduct.Images.Add(new Image
                {
                    Url = productVM.PrimaryImage.SaveImage(_env, "Uploads/Product"),
                    IsPrimary = true
                }
                );
            }


            if (productVM.ImagesUrls is not null)
            {
                foreach (var item in existProduct.Images.Where(i => !i.IsPrimary))
                {
                    if (!productVM.ImagesUrls.Any(i => i == item.Url))
                    {
                        ImageExtension.DeleteImage(item.Url, _env, "Uploads/Product");
                        _db.Images.Remove(item);
                    }
                }
            }
            else
            {
                foreach (var item in existProduct.Images.Where(i => !i.IsPrimary))
                {
                    ImageExtension.DeleteImage(item.Url, _env, "Uploads/Product");
                    _db.Images.Remove(item);
                }
            }


            if (productVM.Images is not null)
            {
                foreach (var item in productVM.Images)
                {
                    if (!item.ContentType.Contains("image/"))
                    {
                        ModelState.AddModelError("Images", "File type must be image");
                        return View(productVM);
                    }
                    if (item.Length > 2 * 1024 * 1024)
                    {
                        ModelState.AddModelError("Images", "File size must be less than 2MB");
                        return View(productVM);
                    }
                    existProduct.Images.Add(new Image
                    {
                        Url = item.SaveImage(_env, "Uploads/Product"),
                        IsPrimary = false
                    });
                }
            }

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