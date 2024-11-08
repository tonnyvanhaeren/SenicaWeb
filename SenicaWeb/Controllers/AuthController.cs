using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SenicaWeb.Constants;
using SenicaWeb.Models;
using SenicaWeb.Models.Auth;

namespace SenicaWeb.Controllers
{
    [Authorize]
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IPasswordHasher<AppUser> _passwordHasher;


        public AuthController(UserManager<AppUser> userManager, IPasswordHasher<AppUser> passwordHash)
        {
            _userManager = userManager;
            _passwordHasher = passwordHash;
        }

        // GET: AuthController
        public ActionResult Index()
        {
            return View(_userManager.Users);
        }

        [Authorize(Roles = Roles.ALL_ROLES)]
        public ActionResult Profiel()
        {
            return View();
        }

        [AllowAnonymous]
        public ViewResult Create() => View();

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create(RegisterInput user)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = new AppUser
                {
                    UserName = user.Email,
                    Email = user.Email,
                    Voornaam = user.Voornaam,
                    Familienaam = user.Familienaam,
                    GsmNr = user.GsmNr
                    //TwoFactorEnabled = true
                };

                IdentityResult result = await _userManager.CreateAsync(appUser, user.Password!);

                /*if (result.Succeeded)
                {
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(appUser);
                    var confirmationLink = Url.Action("ConfirmEmail", "Email", new { token, email = user.Email }, Request.Scheme);
                    EmailHelper emailHelper = new EmailHelper();
                    bool emailResponse = emailHelper.SendEmail(user.Email, confirmationLink);

                    if (emailResponse)
                        return RedirectToAction("Index");
                    else
                    {
                        // log email failed 
                    }
                }*/

                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                {
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
            return View(user);
        }

        public async Task<IActionResult> Update(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
                return View(user);
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, string email, string password, string voornaam, string familienaam, string gsmNr)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(voornaam))
                    user.Voornaam = voornaam;
                else
                {
                    ModelState.AddModelError("", "Voornaam cannot be empty");
                }

                if (!string.IsNullOrEmpty(familienaam))
                    user.Familienaam = familienaam;
                else
                {
                    ModelState.AddModelError("", "Familienaam cannot be empty");
                }

                if (!string.IsNullOrEmpty(gsmNr))
                    user.GsmNr = gsmNr;
                else
                {
                    ModelState.AddModelError("", "Gsm nummer cannot be empty");
                }

/*                 if (!string.IsNullOrEmpty(email))
                    user.Email = email;
                else
                    ModelState.AddModelError("", "Email cannot be empty"); */


                
                if (!string.IsNullOrEmpty(password))

                    user.PasswordHash = _passwordHasher.HashPassword(user, password);
                else
                    ModelState.AddModelError("", "Password cannot be empty");

                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
                {
                    IdentityResult result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return RedirectToAction("Index");
                    else
                        Errors(result);
                }
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return View(user);
        }

        [Authorize(Roles = Roles.ADMIN)]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(result);
            }
            else
                ModelState.AddModelError("", "User Not Found");
                return View("Index", _userManager.Users);
        }

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
    }
}
