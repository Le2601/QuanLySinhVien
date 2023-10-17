﻿
using Microsoft.AspNetCore.Authorization;
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


namespace QuanLySinhVien.Controllers.Students
{
    [Authorize(Roles = "Student")]
    public class UploadAssignmentController : Controller
    {
        private readonly ElearingDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public UploadAssignmentController(ElearingDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
      

        //upload file
        [HttpPost]
        public IActionResult UploadAssignment(IFormFile file, int id)
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

                //end luu du lieu vao wwwroot


                //lay session vao trang admin

                string loginStudent = HttpContext.Session.GetString("AccountId_Student");

                int soNguyen = int.Parse(loginStudent);


                // Lưu dữ liệu vào cơ sở dữ liệu

                var newDocument = new UploadAssignment
                {
                    ExerciseContentId = id,
                    AccountId = soNguyen,
                    Alias = "Null",
                    Data = fileData,
                    UpdateDay = DateTime.Now,
                    DataName = file.FileName,

                };


                _context.UploadAssignments.Add(newDocument);
                _context.SaveChanges();



                // Trả về thông báo thành công hoặc dữ liệu JSON (tùy chọn)
                return Ok(new { Message = "File uploaded successfully" });
            }


            return BadRequest();
        }


    }
}
