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
using QuanLySinhVien.DI.CourseMembers;

namespace QuanLySinhVien.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CourseMemberController : Controller
    {

        private readonly ICourseMemberRepository _CourseMemberRepository;

        private readonly IWebHostEnvironment _environment;

        public CourseMemberController(ICourseMemberRepository courseRepository, IWebHostEnvironment environment)
        {
            _CourseMemberRepository = courseRepository;


            _environment = environment;


        }


        public async Task<ActionResult<IEnumerable<CourseMember>>> Index()
        {
            var items = await _CourseMemberRepository.GetAll();

            ViewBag.CourseId = new SelectList(_CourseMemberRepository.GetAllCourse(),"Id","Title");

            return View(items);
        }

        public IActionResult Create()
        {
            ViewBag.CourseId = new SelectList(_CourseMemberRepository.GetAllCourse(), "Id", "Title");

            return View();
        }


        [HttpPost]

        public async Task<IActionResult> Create(CourseMember model)
        {
          if (ModelState.IsValid)
            {

                
                var create = await _CourseMemberRepository.Create(model);

                return RedirectToAction("index");
                
            }

          return View(model);
        }

    }
}
