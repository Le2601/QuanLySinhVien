using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using QuanLySinhVien.Migrations;
using QuanLySinhVien.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace QuanLySinhVien.Controllers.Lecturers
{
    [Authorize(Roles = "Employee")]
    public class ExerciseContentLecturersController : Controller
    {
        private readonly ElearingDbContext _context;

        private readonly IWebHostEnvironment _environment;

        public ExerciseContentLecturersController(ElearingDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;

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

                //var GetAliasCourse = HttpContext.Session.GetString("AliasCourse");

                //var GetAliasContent = _context.CourseContents.Where(x => x.Id == IdCourseContent).FirstOrDefault();


                //return RedirectToAction("Index", new { alias_course = GetAliasCourse, alias = , Id = IdCourseContent });

            }
            return View(model);
        }

        

        [HttpPost]

        public IActionResult Delete(int id)
        {
            var item = _context.ExerciseContents.Find(id);

            //lay ra bai sv dua vao id noi dung thuc hanh
            var item_bt = _context.UploadAssignments.Where(x=> x.ExerciseContentId == id).ToList();

           
            if (ModelState.IsValid && item_bt != null)
            {
                _context.ExerciseContents.Remove(item);

                //lay ra duoc danh sach duyet trong danh sach va xoa tung doi tuong ben trong danh sach do
                //luu du lieu vao wwwroot
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");

                // Tạo folder "newFolder" trong thư mục "uploads"
                string newFolderPath = Path.Combine(uploadsFolder, "UploadAssignment");


                foreach (var getitem in item_bt)
                {
                    //cap nhat xong xoa file cu
                    // Xác định đường dẫn tới tệp tin cần xóa trong thư mục wwwroot
                    string filePath_Delete = Path.Combine(newFolderPath, getitem.DataName);

                    // Kiểm tra xem tệp tin có tồn tại hay không
                    if (System.IO.File.Exists(filePath_Delete))
                    {
                        // Xóa tệp tin
                        System.IO.File.Delete(filePath_Delete);
                    }

                    //xoa bai tap sinh vien da nop
                    _context.UploadAssignments.Remove(getitem);
                }

                    

              

               

                _context.SaveChanges();

          

                return Json(new { success = true });
            }
            return Json(new { success = false });



        }



        //danh sach sv da nop bai
        public async Task<IActionResult> Student_Submit(int id)
        {
            var items = await _context.UploadAssignments.Where(x => x.ExerciseContentId == id).ToListAsync();






            return View(items);
        }

        // xoa bai thuc hanh sinh vien da nop

        public IActionResult Delete_UploadAssignment(int id)
        {
            var item =  _context.UploadAssignments.Where(x=> x.Id==id).FirstOrDefault();

           if (item != null)
            {
                _context.UploadAssignments.Remove(item);
                //lay ra duoc danh sach duyet trong danh sach va xoa tung doi tuong ben trong danh sach do
                //luu du lieu vao wwwroot
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");

                // Tạo folder "newFolder" trong thư mục "uploads"
                string newFolderPath = Path.Combine(uploadsFolder, "UploadAssignment");

                //cap nhat xong xoa file cu
                // Xác định đường dẫn tới tệp tin cần xóa trong thư mục wwwroot
                string filePath_Delete = Path.Combine(newFolderPath, item.DataName);

                // Kiểm tra xem tệp tin có tồn tại hay không
                if (System.IO.File.Exists(filePath_Delete))
                {
                    // Xóa tệp tin
                    System.IO.File.Delete(filePath_Delete);
                }
                _context.SaveChanges();
                return Json(new { success = true });


            }

            return Json(new { success = false });
        }
    }
}
