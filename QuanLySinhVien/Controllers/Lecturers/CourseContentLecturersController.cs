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
    public class CourseContentLecturersController : Controller
    {
        private readonly ElearingDbContext _context;

        private readonly IWebHostEnvironment _environment;

        public CourseContentLecturersController(ElearingDbContext context, IWebHostEnvironment environment)
        {
            _context = context;

            _environment = environment;
        }
        [Route("/CourseContentLecturers/{alias}-{Id}", Name = "CourseContent")]
        public IActionResult Index(string alias,int? id)
        {
            //if (id == null)
            //{

            //     return View();
            // }

            //luu alias vao session 
            HttpContext.Session.SetString("AliasCourse", alias);  //luu id vao session

            var IdCourse = id;


            string strNumber = IdCourse.ToString(); //ep kieu int sang string
            HttpContext.Session.SetString("IdCourseContent", strNumber);  //luu id vao session
           

            ViewBag.Id = id;
           var items = _context.CourseContents.Where(c => c.CourseId == id).OrderBy(x=> x.Location).ToList();


            ViewBag.course = new SelectList(_context.Courses.ToList(), "Id", "Alias");


            return View(items);
        }

        public IActionResult Create() 
        {


           
            //ViewBag.ListCourse = new SelectList(_context.Courses, "Id", "Title");


            return View();
        
        }

        [HttpPost]
        public IActionResult Create(CourseContent model)
        {

            var GetIdCourseMember = HttpContext.Session.GetString("IdCourseContent"); //lay session do luu vao viewbag truyen qua view

            var GetAliasCourse = HttpContext.Session.GetString("AliasCourse");

            string strNumber = GetIdCourseMember;
            int IdCourse = int.Parse(strNumber);

            if (ModelState.IsValid)
            {
                model.Alias = Helpper.Utilities.SEOUrl(model.Title);
                model.CourseId = IdCourse;
               


                _context.CourseContents.Add(model);
                _context.SaveChanges();




                return RedirectToAction("Index", new { alias = GetAliasCourse,Id = IdCourse  });
                //return RedirectToAction("Index");
            }



            //ViewBag.ListCourse = new SelectList(_context.Courses, "Id", "Title");


            return View(model);

        }

       



        [HttpPost]
        public IActionResult UploadFilesCourseContent(IFormFile file, int id)

        {
            if (file != null && file.Length > 0)
            {
                // Kiểm tra định dạng của tệp tin
                var fileExtension = Path.GetExtension(file.FileName).ToLower();
                if (fileExtension != ".zip" && fileExtension != ".docx" && fileExtension != ".rar")
                {
                    return BadRequest("Invalid file format");
                }

                // Đọc dữ liệu từ tệp tin
                byte[] fileData;
                using (var binaryReader = new BinaryReader(file.OpenReadStream()))
                {
                    fileData = binaryReader.ReadBytes((int)file.Length);
                }

                //luu du lieu vao wwwroot
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");

                // Tạo folder "newFolder" trong thư mục "uploads"
                string newFolderPath = Path.Combine(uploadsFolder, "CourseContentFiles");
                //Directory.CreateDirectory(newFolderPath);


                //// Tạo thư mục uploads nếu nó chưa tồn tại
                //if (!Directory.Exists(uploadsFolder))
                //{
                //    Directory.CreateDirectory(uploadsFolder);
                //}

                // Tạo thư mục uploads nếu nó chưa tồn tại
                if (!Directory.Exists(newFolderPath))
                {
                    Directory.CreateDirectory(newFolderPath);
                }



                // Lấy tên tệp tin gốc của file
                string fileName = file.FileName;
                string filePath = Path.Combine(newFolderPath, fileName);

                // Lưu tệp tin vào thư mục uploads
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                //end luu du lieu vao wwwroot


                // Lưu dữ liệu vào cơ sở dữ liệu


                //truy van den doi tuong CourseContents da tao
                
                var item = _context.CourseContents.Where(x=> x.Id == id).FirstOrDefault();

                //tien hanh them thuoc tinh data va dataname va luu lai ( su dung chinh sua )
                

                item.CourseId = item.CourseId;
                item.Title = item.Title;
                item.Alias = item.Alias;
                item.Data = fileData;
                item.DataName = file.FileName;
                item.Location = item.Location;
                item.IsUpload = item.IsUpload;



                _context.Entry(item).State = EntityState.Modified;


                //_context.CourseContents.Update(newDocument);
                _context.SaveChanges();



                // Trả về thông báo thành công hoặc dữ liệu JSON (tùy chọn)
                return Ok(new { Message = "File uploaded successfully" });
            }


            return BadRequest();
        }

        public async Task<IActionResult> delete(int id)
        {
            var item = await _context.CourseContents.FindAsync(id);

            if (item != null)
            {
                 _context.CourseContents.Remove(item);

                await _context.SaveChangesAsync();

                return Json(new {success = true});

            }
            return Json(new { success = false });
        }

    }
}
