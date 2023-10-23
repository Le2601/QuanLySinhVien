using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
    public class ExerciseContentLecturersController : Controller
    {
        private readonly ElearingDbContext _context;

      

        public ExerciseContentLecturersController(ElearingDbContext context)
        {
            _context = context;

           
        }
        [Route("/{alias_course}/{alias}-{Id}", Name = "ExerciseContentLecturers")]
        public IActionResult Index(int? id)
        {
            //ExerciseContentLecturers
            var IdCourse = id;


            string strNumber = IdCourse.ToString(); //ep kieu int sang string
            HttpContext.Session.SetString("IdCourseExercise", strNumber);  //luu id vao session

            var items = _context.ExerciseContents.Where(c => c.CourseContentId == id).OrderByDescending(x=>x.Location).ToList();
            return View(items);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(ExerciseContent model)
        {
            var GetIdCourseContent = HttpContext.Session.GetString("IdCourseExercise"); //lay session do luu vao viewbag truyen qua view
            string strNumber = GetIdCourseContent;
            int IdCourseContent = int.Parse(strNumber);


            if (ModelState.IsValid)
            {
                model.Alias = Helpper.Utilities.SEOUrl(model.Title);
                model.CourseContentId = IdCourseContent;

                _context.ExerciseContents.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index", "CourseLecturers");

            }
            return View(model);
        }

        //danh sach sv da nop bai
        public async Task<IActionResult> Student_Submit(int id)
        {
            var items = await _context.UploadAssignments.Where(x=> x.ExerciseContentId == id).ToListAsync();


           



            return View(items);
        }

        [HttpPost]

        public IActionResult Delete(int id)
        {
            var item = _context.ExerciseContents.Find(id);

            //xoa bai tap sinh vien da nop

            var item_bt = _context.UploadAssignments.Where(x=> x.ExerciseContentId == id).FirstOrDefault();


            if (ModelState.IsValid && item_bt != null)
            {
                _context.ExerciseContents.Remove(item);

                _context.UploadAssignments.Remove(item_bt);

                _context.SaveChanges();

          

                return Json(new { success = true });
            }
            return Json(new { success = false });



        }
    }
}
