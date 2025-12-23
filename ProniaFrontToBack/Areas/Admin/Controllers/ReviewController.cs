using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaFrontToBack.Areas.Admin.ViewModels.Review;
using ProniaFrontToBack.DAL;
using ProniaFrontToBack.Models;

namespace ProniaFrontToBack.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReviewController : Controller
    {
        AppDbContext _db;
        public ReviewController(AppDbContext db)
        {
            _db = db;
        }
        #region Index
        public IActionResult Index()
        {
            var review = _db.Reviews
            .ToList();
            return View(review);
        }
        #endregion

        #region Create (GET)
        public IActionResult Create()
        {
            ViewBag.Products = _db.Products.ToList();
            return View();
        }
        #endregion

        #region Create (POST)
        [HttpPost]
        public async Task<IActionResult> Create(CreateReviewVM reviewVM)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Products = _db.Products.ToList();
                return View(reviewVM);
            }
            Review review = new Review
            {
                Comment = reviewVM.Comment,
                ProductId = reviewVM.ProductId
            };
            await _db.Reviews.AddAsync(review);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Soft Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return View("Error");
            var review = await _db.Reviews.FirstOrDefaultAsync(r => r.Id == id);
            if (review == null) return View("Error");
            review.IsDeleted = true;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Restore
        [HttpPost]
        public async Task<IActionResult> Restore(int? id)
        {
            if (id == null) return View("Error");
            var review = await _db.Reviews.FirstOrDefaultAsync(r => r.Id == id);
            if (review == null) return View("Error");
            review.IsDeleted = false;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Update (GET)
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return View("Error");
            var review = await _db.Reviews.FirstOrDefaultAsync(r => r.Id == id);
            if (review == null) return View("Error");
            ViewBag.Products = _db.Products.ToList();
            UpdateReviewVM reviewVM = new UpdateReviewVM()
            {
                Id = review.Id,
                Comment = review.Comment,
                ProductId = review.ProductId
            };
            return View(reviewVM);
        }
        #endregion

        #region Update (POST)
        [HttpPost]
        public async Task<IActionResult> Update(UpdateReviewVM reviewVM)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Products = _db.Products.ToList();
                return View(reviewVM);
            }
            var review = await _db.Reviews.FirstOrDefaultAsync(r => r.Id == reviewVM.Id);
            if (review == null) return View("Error");
            review.Comment = reviewVM.Comment;
            review.ProductId = reviewVM.ProductId;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
    }
}