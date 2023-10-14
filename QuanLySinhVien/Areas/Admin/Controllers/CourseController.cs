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

namespace QuanLySinhVien.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize] //bat xac thuc trc khi vao trang nay

    public class CourseController : Controller
    {
        private readonly ICourseRepository _CourseRepository;

        private readonly IWebHostEnvironment _environment;

        public CourseController(ICourseRepository courseRepository, IWebHostEnvironment environment)
        {
            _CourseRepository = courseRepository;


            _environment = environment;


        }

    



        public async Task<ActionResult<IEnumerable<Course>>> Index()
        {
            var items = await _CourseRepository.GetCourses();

            ViewBag.department = new SelectList(_CourseRepository.GetDepartments(), "Id", "Title");
            ViewBag.semester = new SelectList(_CourseRepository.GetSemesterCourses(), "Id", "Title");


            ViewBag.AccountName = new SelectList(_CourseRepository.GetAllCreator(), "Id", "Title");


            return View(items);
        }


        public IActionResult Create()
        {
            ViewBag.department = new SelectList(_CourseRepository.GetDepartments(),"Id","Title");
            ViewBag.semester = new SelectList(_CourseRepository.GetSemesterCourses(), "Id", "Title");
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




        [HttpPost]
        public async Task<IActionResult> UploadFiles(IFormFile file,int id)
        {

            var IdUploadFile = id;

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
                string newFolderPath = Path.Combine(uploadsFolder, "Demo");
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

                //end luu du lieu vao wwwroot.


                // Lưu dữ liệu vào cơ sở dữ liệu

                var newDocument = new UpLoadFileTLL
                {
                    Title = file.FileName,
                    Data = fileData,
                    CourseId = IdUploadFile
                };

                var CreateI = await _CourseRepository.CreateTl(newDocument);


              
                

                // Trả về thông báo thành công hoặc dữ liệu JSON (tùy chọn)
                return Ok(new { Message = "File uploaded successfully" });
            }

            // Trả về thông báo lỗi hoặc dữ liệu JSON (tùy chọn)
            return BadRequest();
        }

        public async Task<ActionResult<IEnumerable<UpLoadFileTLL>>> demoListTl()
        {
            var items = await _CourseRepository.GetTls();

          

            return View(items);
        }

      

        
            


    }
}
