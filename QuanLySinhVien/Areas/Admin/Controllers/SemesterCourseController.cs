using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using QuanLySinhVien.Models;
//using QuanLySinhVien.ModelView;
//using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using QuanLySinhVien.DI.Departments;
using QuanLySinhVien.DI.Roles;
using QuanLySinhVien.DI.SemesterCourses;

namespace QuanLySinhVien.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Employee,Admin")]
    public class SemesterCourseController : Controller
    {

        private readonly ISemesterCoursesRepository _semesterCoursesRepository;

        public SemesterCourseController(ISemesterCoursesRepository semesterCoursesRepository)
        {
            _semesterCoursesRepository = semesterCoursesRepository;
        }


       public async Task<ActionResult<IEnumerable<SemesterCourse>>> Index()
        {
            var items =await _semesterCoursesRepository.GetSemesterCourses();

            return View(items);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(SemesterCourse model)
        {
            if (ModelState.IsValid)
            {

                model.Alias = QuanLySinhVien.Helpper.Utilities.SEOUrl(model.Title);

                var create = await _semesterCoursesRepository.Create(model);

                return RedirectToAction("Index");

            }
            return View(model);


        }
    }
}
