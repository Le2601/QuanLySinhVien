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
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace QuanLySinhVien.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SemesterCourseController : Controller
    {

        private readonly ISemesterCoursesRepository _semesterCoursesRepository;
        private readonly ElearingDbContext _context;
        public SemesterCourseController(ElearingDbContext context, ISemesterCoursesRepository semesterCoursesRepository)
        {
            _context = context;
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
            var ListCourse = _context.SemesterCourse.Where(x => x.Title == model.Title).ToList();

            if (ModelState.IsValid)
            {
                if (ListCourse.Count > 0)
                {
                    ModelState.AddModelError("Title", "Tên đã tồn tại.");
                   
                    return View(model);
                }
                model.Alias = QuanLySinhVien.Helpper.Utilities.SEOUrl(model.Title);

                var create = await _semesterCoursesRepository.Create(model);

                return RedirectToAction("Index");

            }
            return View(model);


        }

        public async Task<IActionResult> Update(int id)
        {
            var item =await _semesterCoursesRepository.GetSemesterCourse(id);
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Update(SemesterCourse model)
        {

            if (ModelState.IsValid)
            {
                model.Alias = QuanLySinhVien.Helpper.Utilities.SEOUrl(model.Title);

                var Update =await _semesterCoursesRepository.Update(model);
                return RedirectToAction("Index");
            }
            return View(model);

        }

        [HttpPost]

        public async Task<IActionResult> Delete(int id)
        {

            var item = await _semesterCoursesRepository.GetSemesterCourse(id);
            //kiem tra neu khoa da ton tai khoa hoc

            var CheckCourse = _context.Courses.Where(x => x.SemesterCourseId == id).ToList();

            if (CheckCourse.Count >= 1)
            {
                return Json(new { success = false, msg = "Tồn tại khóa ngoại không thể xóa" });
            }
            if (item != null)
            {
                 await _semesterCoursesRepository.Delete(item);

                return Json(new {success = true});
            }
            return Json(new { success = false });

        }
    }
}
