using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public IActionResult Student_Submit(int id)
        {
            var items = _context.UploadAssignments.Where(x=> x.ExerciseContentId == id).ToList();


            ViewBag.account = new SelectList(_context.Account.ToList(), "Id", "Title");



            return View(items);
        }
    }
}
