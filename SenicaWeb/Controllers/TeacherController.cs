using Microsoft.AspNetCore.Mvc;
using SenicaWeb.Data;
using SenicaWeb.Models;
using SenicaWeb.Models.Teachers;

namespace SenicaWeb.Controllers
{
    public class TeacherController(SenicaDbContext dbContext) : Controller
    {
        private readonly SenicaDbContext _dbContext = dbContext;
        
        [HttpGet]
        public ActionResult Index()
        {
            return View(_dbContext.Teachers.ToList());
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeacherInput input) {
            if(ModelState.IsValid)
            {
                Teacher newTeacher = new Teacher
                {
                    TeacherId = Guid.NewGuid().ToString(),
                    FotoUrl = input.FotoUrl,
                    Voornaam = input.Voornaam,
                    Familienaam = input.Familienaam,
                    Email = input.Email,
                    GsmNr = input.GsmNr
                };
                
                Console.WriteLine(newTeacher.TeacherId);
                
                await _dbContext.Teachers.AddAsync(newTeacher);
                await _dbContext.SaveChangesAsync();
                
                return RedirectToAction("Index", "Teacher");
            }
            return View(input);
        }

    }
}
