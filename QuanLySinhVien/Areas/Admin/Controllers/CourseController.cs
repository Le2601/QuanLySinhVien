using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuanLySinhVien.DI.Courses;
using QuanLySinhVien.DI.Roles;
using QuanLySinhVien.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuanLySinhVien.Helpper;
using System;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Drawing2D;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagedList.Core;
using System.Linq;

namespace QuanLySinhVien.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class CourseController : Controller
    {
        private readonly ICourseRepository _CourseRepository;
        private readonly ElearingDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public CourseController(ElearingDbContext context, ICourseRepository courseRepository, IWebHostEnvironment environment)
        {
            _CourseRepository = courseRepository;
            _context = context;

            _environment = environment;


        }

    



        public async Task<ActionResult<IEnumerable<Course>>> Index(int? page)
        {
            var items =  _context.Courses.OrderBy(x=> x.Id);

            ViewBag.department = new SelectList(_CourseRepository.GetDepartments(), "Id", "Title");
            ViewBag.semester = new SelectList(_CourseRepository.GetSemesterCourses(), "Id", "Title");


            ViewBag.AccountName = new SelectList(_CourseRepository.GetAllCreator(), "Id", "Title");



            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 6;
            

            PagedList<Course> models = new PagedList<Course>(items, pageNumber, pageSize);

            return View(models);
        }


        public IActionResult Create()
        {
            ViewBag.department = new SelectList(_CourseRepository.GetDepartments(),"Id","Title");
            ViewBag.semester = new SelectList(_CourseRepository.GetSemesterCourses(), "Id", "Title");

            ViewBag.Account = new SelectList(_CourseRepository.GetAllCreator(), "Id", "FullName");

           
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Course model)
        {


            ViewBag.department = new SelectList(_CourseRepository.GetDepartments(), "Id", "Title");
            ViewBag.semester = new SelectList(_CourseRepository.GetSemesterCourses(), "Id", "Title");

            ViewBag.Account = new SelectList(_CourseRepository.GetAllCreator(), "Id", "FullName");


            var ListCourse = _context.Courses.Where(x => x.Title == model.Title).ToList();

                if (model.DepartmentId == 0)
                {

               
                    ModelState.AddModelError("DepartmentId", "Không thể để trống");
                    return View(model);
                }
                if (model.SemesterCourseId == 0)
                {


                    ModelState.AddModelError("SemesterCourseId", "Không thể để trống");
                    return View(model);
                }
               


            if (ModelState.IsValid)
            {
                

                //kiem tra ten da ton tai
                if (ListCourse.Count > 0)
                {
                    ModelState.AddModelError("Title", "Tên đã tồn tại.");
                    ViewBag.department = new SelectList(_CourseRepository.GetDepartments(), "Id", "Title");
                    ViewBag.semester = new SelectList(_CourseRepository.GetSemesterCourses(), "Id", "Title");

                    ViewBag.Account = new SelectList(_CourseRepository.GetAllCreator(), "Id", "FullName");
                    return View(model);
                }

                model.AccountId = model.AccountId;

                var GetNameGv = _context.Account.Where(x => x.Id == model.AccountId).FirstOrDefault();

                // xử lý lưu tiêu đề  + tên giảng viên



                var EditTitle = model.Title + "-" + GetNameGv.FullName;

                model.Title = EditTitle;

                model.Alias = Helpper.Utilities.SEOUrl(EditTitle);

            








                var CreateI = await _CourseRepository.CreateCourse(model);



                return RedirectToAction("index");
            }
            return View(model);
          


        }

        public async Task<IActionResult> Update(int id)
        {

            ViewBag.DepartmentId = new SelectList(_CourseRepository.GetDepartments(), "Id", "Title");
            ViewBag.SemesterCourseId = new SelectList(_CourseRepository.GetSemesterCourses(), "Id", "Title");

            ViewBag.Account = new SelectList(_CourseRepository.GetAllCreator(), "Id", "FullName");

            var item = await _CourseRepository.GetCourse(id);

            return View(item);
           



        }
        [HttpPost]
        public async Task<IActionResult> Update(Course model)
        {
            ViewBag.DepartmentId = new SelectList(_CourseRepository.GetDepartments(), "Id", "Title");
            ViewBag.SemesterCourseId = new SelectList(_CourseRepository.GetSemesterCourses(), "Id", "Title");

            ViewBag.Account = new SelectList(_CourseRepository.GetAllCreator(), "Id", "FullName");

            if (ModelState.IsValid)
            {
               model.Alias = Helpper.Utilities.SEOUrl(model.Title);
              await _CourseRepository.EditCourse(model);

              return RedirectToAction("index");


            }
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {

            Course course =await _CourseRepository.GetCourse(id);

            //kiem tra neu khoa da ton tai khoa hoc

            var CheckCourseContent = _context.CourseContents.Where(x => x.CourseId == id).ToList();
            var CheckCourseMember = _context.CourseMembers.Where(x => x.CourseId == id).ToList();

            if (CheckCourseContent.Count >= 1 || CheckCourseMember.Count >= 1)
            {
                return Json(new { success = false, msg = "Tồn tại khóa ngoại không thể xóa" });
            }



            if (course != null)
            {
                await _CourseRepository.DeleteCourse(course);
                return Json(new { success = true });
            }
            return Json(new { success = false });


        }

        public async Task<IActionResult> UploadsFile(int id)
        {
            var IdCourse = await _CourseRepository.GetCourse(id);

            ViewBag.IdUploadFile = id;

            return View(IdCourse);
        }


        [Route("/chi-tiet/{Alias}-{Id}", Name = "chi-tiet")]
        public async Task<IActionResult> Detail_Course(int id)
        {

            ViewBag.department = new SelectList(_CourseRepository.GetDepartments(), "Id", "Title");
            ViewBag.semester = new SelectList(_CourseRepository.GetSemesterCourses(), "Id", "Title");
            ViewBag.Account = new SelectList(_CourseRepository.GetAllCreator(), "Id", "FullName");

            var item = await _context.Courses.Where(x => x.Id == id).FirstOrDefaultAsync();

            //diem so luong sinh vien trong khoa hoc

            var CountStudent = _context.CourseMembers.Where(x=> x.CourseId == id).Count();

            ViewBag.CountStudent = CountStudent;

            return View(item);

        }












    }
}
