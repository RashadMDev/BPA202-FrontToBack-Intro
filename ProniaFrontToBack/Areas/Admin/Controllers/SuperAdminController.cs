using Microsoft.AspNetCore.Identity;
using ProniaFrontToBack.Utilities.Account;

namespace ProniaFrontToBack.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin")]
    public class SuperAdminController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        public SuperAdminController(AppDbContext db, UserManager<AppUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.Where(u => !u.IsAdmin).ToListAsync();
            return View(users);
        }
        public async Task<IActionResult> Admins()
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            return View(admins);
        }

        public IActionResult CreateAdmin()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAdmin(AdminRegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Please correct the errors and try again.");
                return View();
            }

            AppUser appUser = new()
            {
                Name = "Admin",
                Surname = "Admin",
                UserName = registerVM.Email,
                Email = registerVM.Email,
            };
            appUser.IsAdmin = true;

            IdentityResult result = await _userManager.CreateAsync(appUser, registerVM.Password);
            await _userManager.AddToRoleAsync(appUser, Roles.Admin.ToString());

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    return View();
                }
            }

            return RedirectToAction("Admins");
        }


    }
}