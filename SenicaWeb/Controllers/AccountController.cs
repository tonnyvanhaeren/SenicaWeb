using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SenicaWeb.Models;
using SenicaWeb.Models.Auth;

namespace SenicaWeb.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signinManager;

        public AccountController(UserManager<AppUser> userMgr, SignInManager<AppUser> signinMgr)
        {
            _userManager = userMgr;
            _signinManager = signinMgr;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            LoginInput login = new LoginInput();
            login.ReturnUrl = returnUrl;
            return View(login);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInput login)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = await _userManager.FindByEmailAsync(login.Email);
                if (appUser != null)
                {
                    await _signinManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await _signinManager.PasswordSignInAsync(appUser, login.Password, login.Remember, false);
                    if (result.Succeeded)
                        return Redirect(login.ReturnUrl ?? "/");
                }
                ModelState.AddModelError(nameof(login.Email), "Login Failed: Invalid Email or password");
            }
            return View(login);
        }
    
    public async Task<IActionResult> Logout()
    {
        await _signinManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
    
    public IActionResult AccessDenied()
    {
        return View();
    }

    }
}
