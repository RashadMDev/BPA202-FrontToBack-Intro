using Microsoft.AspNetCore.Identity;
using ProniaFrontToBack.Interfaces;
using ProniaFrontToBack.Utilities.Account;

namespace ProniaFrontToBack.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMailService _mailService;
        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IMailService mailService
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mailService = mailService;
        }

        #region User Register
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
        #endregion

        #region User Login
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
        #endregion

        #region Logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Admin Register
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
        #endregion

        #region Admin Login
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
        #endregion

        public IActionResult ForgottenPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgottenPassword(ForgotVM forgotVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Please correct the errors and try again.");
                return View(forgotVM);
            }
            AppUser appUser = await _userManager.FindByEmailAsync(forgotVM.Email);
            if (appUser is null)
            {
                ModelState.AddModelError(string.Empty, "There is no user with this email address.");
                return View(forgotVM);
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(appUser);
            object userStat = new
            {
                userId = appUser.Id,
                token = token
            };
            var link = Url.Action("ResetPassword", "Account", userStat, HttpContext.Request.Scheme);

            var emailBody = $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Reset Your Password</title>
    <style>
        body {{
            margin: 0;
            padding: 0;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background-color: #f4f4f4;
        }}
        .email-container {{
            max-width: 600px;
            margin: 0 auto;
            background-color: #ffffff;
        }}
        .header {{
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            padding: 40px 20px;
            text-align: center;
        }}
        .header h1 {{
            color: #ffffff;
            margin: 0;
            font-size: 28px;
            font-weight: 600;
        }}
        .content {{
            padding: 40px 30px;
            color: #333333;
        }}
        .content h2 {{
            color: #333333;
            font-size: 24px;
            margin-bottom: 20px;
        }}
        .content p {{
            color: #666666;
            font-size: 16px;
            line-height: 1.6;
            margin-bottom: 20px;
        }}
        .button-container {{
            text-align: center;
            margin: 35px 0;
        }}
        .reset-button {{
            display: inline-block;
            padding: 16px 40px;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: #ffffff !important;
            text-decoration: none;
            border-radius: 30px;
            font-weight: 600;
            font-size: 16px;
            box-shadow: 0 4px 15px rgba(102, 126, 234, 0.4);
            transition: all 0.3s ease;
        }}
        .reset-button:hover {{
            box-shadow: 0 6px 20px rgba(102, 126, 234, 0.6);
            transform: translateY(-2px);
        }}
        .info-box {{
            background-color: #f8f9fa;
            border-left: 4px solid #667eea;
            padding: 15px 20px;
            margin: 25px 0;
            border-radius: 4px;
        }}
        .info-box p {{
            margin: 0;
            font-size: 14px;
            color: #555555;
        }}
        .footer {{
            background-color: #f8f9fa;
            padding: 30px;
            text-align: center;
            color: #999999;
            font-size: 14px;
        }}
        .footer p {{
            margin: 5px 0;
            color: #999999;
        }}
        .footer a {{
            color: #667eea;
            text-decoration: none;
        }}
        .divider {{
            height: 1px;
            background-color: #e0e0e0;
            margin: 30px 0;
        }}
    </style>
</head>
<body>
    <div class='email-container'>
        <div class='header'>
            <h1>🔐 Password Reset Request</h1>
        </div>
        
        <div class='content'>
            <h2>Hello, {appUser.Name}!</h2>
            <p>We received a request to reset your password. Don't worry, we're here to help you get back into your account.</p>
            
            <div class='button-container'>
                <a href='{link}' class='reset-button'>Reset My Password</a>
            </div>
            
            <div class='info-box'>
                <p><strong>⏰ Important:</strong> This link will expire in 24 hours for security reasons.</p>
            </div>
            
            <p>If the button doesn't work, you can copy and paste this link into your browser:</p>
            <p style='word-break: break-all; color: #667eea; font-size: 14px;'>{link}</p>
            
            <div class='divider'></div>
            
            <p style='font-size: 14px;'><strong>Didn't request this?</strong></p>
            <p style='font-size: 14px;'>If you didn't ask to reset your password, you can safely ignore this email. Your password will remain unchanged.</p>
        </div>
        
        <div class='footer'>
            <p><strong>Pronia Team</strong></p>
            <p>Need help? Contact us at <a href='mailto:support@pronia.com'>support@pronia.com</a></p>
            <p style='margin-top: 15px;'>© 2026 Pronia. All rights reserved.</p>
        </div>
    </div>
</body>
</html>";

            await _mailService.SendEmailAsync(new Utilities.Email.MailRequest
            {
                ToEmail = appUser.Email,
                Subject = "Reset Your Password - Pronia",
                Body = emailBody
            });
            return View();
        }

        public IActionResult ResetPassword(string userId, string token)
        {
            if (userId is null)
            {
                ModelState.AddModelError(string.Empty, "Invalid user.");
                return View();
            }
            ResetVM resetVM = new()
            {
                Token = token,
                UserId = userId,
            };
            return View(resetVM);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetVM resetVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Please correct the errors and try again.");
                return View(resetVM);
            }
            AppUser appUser = await _userManager.FindByIdAsync(resetVM.UserId);
            if (appUser is null)
            {
                ModelState.AddModelError(string.Empty, "Invalid user.");
                return View(resetVM);
            }
            var result = await _userManager.ResetPasswordAsync(appUser, resetVM.Token, resetVM.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(resetVM);
            }
            return RedirectToAction("Login");
        }

        #region Create Role
        // public async Task<IActionResult> CreateRole()
        // {
        //     foreach (var role in Enum.GetNames(typeof(Roles)))
        //     {
        //         await _roleManager.CreateAsync(new IdentityRole(role));
        //     }

        //     return Content("Role Created");
        // }
        #endregion
    }
}