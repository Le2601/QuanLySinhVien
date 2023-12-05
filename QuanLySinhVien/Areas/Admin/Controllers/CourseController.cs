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

            if (ModelState.IsValid)
            {


                model.Alias = Helpper.Utilities.SEOUrl(model.Title);

                //lay session vao trang admin

                string loginAdmin = HttpContext.Session.GetString("AccountId");

                ViewData["AccountId"] = loginAdmin;       

                int IdCreate = Int32.Parse(loginAdmin); //ep kieu string sang int

                model.AccountId = IdCreate; //luu bien vua ep vao thuoc tinh creator

         

                ////lay id cua nguoi tao 

                //var GetNameCreator = _CourseRepository.GetNameCreator(IdCreate);



                




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




        
       

      

        
            


    }
}
