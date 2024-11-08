using Microsoft.AspNetCore.Mvc;
using SenicaWeb.Models.Teachers;

namespace SenicaWeb.Controllers
{
    public class TeacherController : Controller
    {
        // GET: TeacherController
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeacherInput input) {
            if(ModelState.IsValid){
                return RedirectToAction("Index", "Teacher");
            }
            return View(input);
        }

    }
}
