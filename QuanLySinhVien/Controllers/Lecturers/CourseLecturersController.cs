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
        public IActionResult Index()
        {

            var taikhoan = HttpContext.Session.GetString("AccountId_Lecturers");

            int IdAccount = Int32.Parse(taikhoan); //ep kieu string sang int


            ViewBag.semester = new SelectList(_context.SemesterCourse.ToList(), "Id", "Title");

            var items = _context.Courses.Where(x => x.AccountId == IdAccount).ToList();



            return View(items);
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
                    if(ListCourse != null)
                    {
                        ModelState.AddModelError("Title", "Tên đã tồn tại.");
                        ViewBag.department = new SelectList(_context.Department.ToList(), "Id", "Title");
                        ViewBag.semester = new SelectList(_context.SemesterCourse.ToList(), "Id", "Title");
                        return View(model);
                      }


                    model.Alias = Helpper.Utilities.SEOUrl(model.Title);

                        var taikhoan = HttpContext.Session.GetString("AccountId_Lecturers");

                        int IdAccount = Int32.Parse(taikhoan); //ep kieu string sang int

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

            if (ModelState.IsValid)
            {

                _context.Courses.Remove(item);
                _context.SaveChanges();
                return Json(new { success = true });
               

            }

            return Json(new { success = false });



        }



        [ResponseCache(Duration = 60)] // Caching trong 60 giây
        //upload file
        [HttpPost]
        public async Task<IActionResult> UploadAssignment(IFormFile file, int id)
        {


            if (file != null && file.Length > 0)
            {
                // Kiểm tra định dạng của tệp tin
                var fileExtension = Path.GetExtension(file.FileName).ToLower();
                if (fileExtension != ".zip" && fileExtension != ".docx" && fileExtension != ".rar" && fileExtension != ".png")
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
                string newFolderPath = Path.Combine(uploadsFolder, "UploadAssignment");
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

                //end luu du lieu vao wwwroot2


                //lay session vao trang admin

                string loginStudent = HttpContext.Session.GetString("AccountId_Student");

                int soNguyen = int.Parse(loginStudent);

                var get_Account = _context.Account.Where(x => x.Id == soNguyen).FirstOrDefault();

                //kiem tra xem da co ton tai trong db UploadAssignment chuwa neu co roi thi update chua thi add

                var add_or_update = _context.UploadAssignments.Where(x => x.Mssv.Equals(get_Account.Code) && x.ExerciseContentId == id).FirstOrDefault();



                if (add_or_update != null)
                {
                    //cap nhat xong xoa file cu
                    // Xác định đường dẫn tới tệp tin cần xóa trong thư mục wwwroot
                    string filePath_Delete = Path.Combine(newFolderPath, add_or_update.DataName);

                    // Kiểm tra xem tệp tin có tồn tại hay không
                    if (System.IO.File.Exists(filePath_Delete))
                    {
                        // Xóa tệp tin
                        System.IO.File.Delete(filePath_Delete);
                    }



                    // Cập nhật nội dung (update)
                    add_or_update.Alias = "Null";
                    add_or_update.Data = fileData;
                    add_or_update.UpdateDay = DateTime.Now;
                    add_or_update.DataName = file.FileName;



                    _context.Attach(add_or_update);
                    _context.Entry(add_or_update).State = EntityState.Modified;

                }



                else
                {
                    var newDocument = new UploadAssignment
                    {
                        ExerciseContentId = id,
                        FullName = get_Account.FullName,
                        Mssv = get_Account.Code,
                        Alias = "Null",
                        Data = fileData,
                        UpdateDay = DateTime.Now,
                        DataName = file.FileName,

                    };
                    await _context.UploadAssignments.AddAsync(newDocument);


                }

                await _context.SaveChangesAsync();



                // Trả về thông báo thành công hoặc dữ liệu JSON (tùy chọn)
                return Ok(new { Message = "File uploaded successfully" });
            }


            return BadRequest();
        }

    }
}
