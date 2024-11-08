using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SenicaWeb.Models;

namespace SenicaWeb.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleMgr;
        private readonly UserManager<AppUser> _userMrg;

        public RoleController(RoleManager<IdentityRole> roleMgr, UserManager<AppUser> userMrg)
        {
            _roleMgr = roleMgr;
            _userMrg = userMrg;
        }
        
        
        public ViewResult Index() => View(_roleMgr.Roles);

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create([Required] string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await _roleMgr.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(result);
            }
            return View(name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await _roleMgr.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await _roleMgr.DeleteAsync(role);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(result);
            }
            else
                ModelState.AddModelError("", "No role found");
            return View("Index", _roleMgr.Roles);
        }        

        public async Task<IActionResult> Update(string id)
        {
            IdentityRole role = await _roleMgr.FindByIdAsync(id);
            List<AppUser> members = new List<AppUser>();
            List<AppUser> nonMembers = new List<AppUser>();
            foreach (AppUser user in _userMrg.Users)
            {
                var list = await _userMrg.IsInRoleAsync(user, role.Name) ? members : nonMembers;
                list.Add(user);
            }
            return View(new RoleEdit
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }

        [HttpPost]
        public async Task<IActionResult> Update(RoleModification model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                foreach (string userId in model.AddIds ?? new string[] { })
                {
                    AppUser user = await _userMrg.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await _userMrg.AddToRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                            Errors(result);
                    }
                }
                foreach (string userId in model.DeleteIds ?? new string[] { })
                {
                    AppUser user = await _userMrg.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await _userMrg.RemoveFromRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                            Errors(result);
                    }
                }
            }

            if (ModelState.IsValid)
                return RedirectToAction(nameof(Index));
            else
                return await Update(model.RoleId);
        }

    }
}
