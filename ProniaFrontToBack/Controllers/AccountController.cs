using Microsoft.AspNetCore.Identity;
using ProniaFrontToBack.Utilities.Account;

namespace ProniaFrontToBack.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Please correct the errors and try again.");
                return View(registerVM);
            }

            AppUser appUser = new()
            {
                Name = registerVM.Name,
                Surname = registerVM.Surname,
                UserName = registerVM.UserName,
                Email = registerVM.Email
            };


            IdentityResult result = await _userManager.CreateAsync(appUser, registerVM.Password);
            await _userManager.AddToRoleAsync(appUser, Roles.User.ToString());

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    return View(registerVM);
                }
            }

            return RedirectToAction("AdminLogin");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM, string? ReturnUrl)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Please correct the errors and try again.");
                return View(loginVM);
            }

            AppUser user = await _userManager.FindByEmailAsync(loginVM.Email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View(loginVM);
            }
            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);

            if (ReturnUrl is not null)
            {
                return Redirect(ReturnUrl);
            }

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View(loginVM);
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult AdminRegister()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AdminRegister(AdminRegisterVM adminRegisterVM)
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
                UserName = adminRegisterVM.Email,
                Email = adminRegisterVM.Email,
            };


            IdentityResult result = await _userManager.CreateAsync(appUser, adminRegisterVM.Password);
            await _userManager.AddToRoleAsync(appUser, Roles.Admin.ToString());

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    return View();
                }
            }

            return RedirectToAction("AdminLogin");
        }
        public IActionResult AdminLogin()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AdminLogin(string email, string password)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Please correct the errors and try again.");
                return View();
            }

            AppUser user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View();
            }
            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        // public async Task<IActionResult> CreateRole()
        // {
        //     foreach (var role in Enum.GetNames(typeof(Roles)))
        //     {
        //         await _roleManager.CreateAsync(new IdentityRole(role));
        //     }

        //     return Content("Role Created");
        // }
    }
}