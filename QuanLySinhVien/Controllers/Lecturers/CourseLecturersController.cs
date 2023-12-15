using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PagedList.Core;
using QuanLySinhVien.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace QuanLySinhVien.Controllers.Lecturers
{
    [Authorize(Roles = "Employee")]
    public class CourseLecturersController : Controller
    {
        private readonly ElearingDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public CourseLecturersController(ElearingDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }


        //[Route("CourseLecturers.html", Name = "Course-giangvien")]
        public IActionResult Index(int? page)
        {

            var taikhoan = HttpContext.Session.GetString("AccountId_Lecturers");

            int IdAccount = Int32.Parse(taikhoan); //ep kieu string sang int

           

            ViewBag.semester = new SelectList(_context.SemesterCourse.ToList(), "Id", "Title");

            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 6;
            var items = _context.Courses.Where(x => x.AccountId == IdAccount);

            PagedList<Course> models = new PagedList<Course>(items, pageNumber, pageSize);

            return View(models);
        }
        public IActionResult Create()
        {
            ViewBag.department = new SelectList(_context.Department.ToList(), "Id", "Title");
            ViewBag.semester = new SelectList(_context.SemesterCourse.ToList(), "Id", "Title");
            return View();
        }
        [HttpPost]
        public IActionResult Create(Course model)
        {

           
            var ListCourse = _context.Courses.Where(x => x.Title == model.Title).ToList();

            

            if (ModelState.IsValid)
                {
                    if(ListCourse.Count > 0  )
                    {
                        ModelState.AddModelError("Title", "Tên đã tồn tại.");
                        ViewBag.department = new SelectList(_context.Department.ToList(), "Id", "Title");
                        ViewBag.semester = new SelectList(_context.SemesterCourse.ToList(), "Id", "Title");
                        return View(model);
                      }


                    

                        var taikhoan = HttpContext.Session.GetString("AccountId_Lecturers");

                        int IdAccount = Int32.Parse(taikhoan); //ep kieu string sang int

                        // xử lý lưu tiêu đề  + tên giảng viên

                        var GetNameGv = _context.Account.Where(x => x.Id == IdAccount).FirstOrDefault();

                         var EditTitle = model.Title +"-"+  GetNameGv.FullName;

                        model.Title = EditTitle;

                        model.Alias = Helpper.Utilities.SEOUrl(EditTitle);


                        model.AccountId = IdAccount; //luu bien vua ep vao thuoc tinh creator

                        _context.Courses.Add(model);
                        _context.SaveChanges();
                        return RedirectToAction("Index");
                }

            


            ViewBag.department = new SelectList(_context.Department.ToList(), "Id", "Title");
            ViewBag.semester = new SelectList(_context.SemesterCourse.ToList(), "Id", "Title");
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.Courses.FindAsync(id);
           if (item == null)
            {
                return NotFound();
            }
            ViewBag.department = new SelectList(_context.Department.ToList(), "Id", "Title");
            ViewBag.semester = new SelectList(_context.SemesterCourse.ToList(), "Id", "Title");

            return View(item);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(Course model)
        {
            if (ModelState.IsValid)
            {
                model.Alias = Helpper.Utilities.SEOUrl(model.Title);

                var taikhoan = HttpContext.Session.GetString("AccountId_Lecturers");

                int IdAccount = Int32.Parse(taikhoan); //ep kieu string sang int

                model.AccountId = IdAccount; //luu bien vua ep vao thuoc tinh creator

                 _context.Courses.Update(model);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            ViewBag.department = new SelectList(_context.Department.ToList(), "Id", "Title");
            ViewBag.semester = new SelectList(_context.SemesterCourse.ToList(), "Id", "Title");
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {

            var item = _context.Courses.Find(id);

            //kiem tra neu khoa da ton tai khoa hoc

            var CheckCourse = _context.CourseContents.Where(x => x.CourseId == id).ToList();

            if (CheckCourse.Count >= 1)
            {
                return Json(new { success = false, msg = "Tồn tại khóa ngoại không thể xóa" });
            }


            if (ModelState.IsValid)
            {

                _context.Courses.Remove(item);
                _context.SaveChanges();
                return Json(new { success = true });
               

            }

            return Json(new { success = false });



        }
        [Route("/chi-tiet_kh_{Alias}-{Id}", Name = "detail_gv")]
        public async Task<IActionResult> details_course(int id)
        {
            ViewBag.department = new SelectList(_context.Department.ToList(), "Id", "Title");
            ViewBag.semester = new SelectList(_context.SemesterCourse.ToList(), "Id", "Title");
            ViewBag.Account = new SelectList(_context.Account.ToList(), "Id", "FullName");

            var item = await _context.Courses.Where(x => x.Id == id).FirstOrDefaultAsync();

            //diem so luong sinh vien trong khoa hoc

            var CountStudent = await _context.CourseMembers.Where(x => x.CourseId == id).CountAsync();

            ViewBag.CountStudent = CountStudent;

            return View(item);
        }



      

    }
}
