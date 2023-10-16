﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using QuanLySinhVien.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;


namespace QuanLySinhVien.Controllers.Lecturers
{
    [Authorize(Roles = "Employee")]
    public class CourseLecturersController : Controller
    {
        private readonly ElearingDbContext _context;

        public CourseLecturersController(ElearingDbContext context)
        {
            _context = context;
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


            if (ModelState.IsValid)
            {
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

    }
}
