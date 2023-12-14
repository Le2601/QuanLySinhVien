using Microsoft.AspNetCore.Authorization;
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
    public class CourseMemberLecturersController : Controller
    {
        private readonly ElearingDbContext _context;

        public CourseMemberLecturersController(ElearingDbContext context)
        {
            _context = context;
        }


        [Route("/CourseMemberLecturers/{alias}-{Id}", Name = "Single")]
        public IActionResult Index(int id)
        {
            var IdCourseMember = id;


            string strNumber = IdCourseMember.ToString(); //ep kieu int sang string
            HttpContext.Session.SetString("IdCourse", strNumber);  //luu id vao session
            var GetIdCourseMember = HttpContext.Session.GetString("IdCourse"); //lay session do luu vao viewbag truyen qua view

            ViewBag.GetIdCourse = GetIdCourseMember;



            var items = _context.CourseMembers.Where(x => x.CourseId == IdCourseMember).ToList();

            ViewBag.getTitleCourse =  _context.Courses.Where(x=> x.Id == IdCourseMember).First().Title;

        

            return View(items);
        }

        public IActionResult Attendance(string id)
        {
            int Id = int.Parse(id);

            var item = _context.CourseMembers.Where(x=> x.CourseId == Id).ToList();


            return View(item);
        }

        [HttpPost]
        public IActionResult Attendance(int id,int Vvalue)
        {
            var item = _context.CourseMembers.Find(id);
            

            if (item != null) {

                if (item.Attendance == null) {

                    item.Attendance = Vvalue;


                }
                else
                {
                    item.Attendance += Vvalue;
                }

                _context.CourseMembers.Update(item);
                _context.SaveChanges();


                return Json(new { success = true, msg = "Check vắng thành công " });
            }


            return Json(new { success = false, msg = "Check vắng thất bại" });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {

            var item = _context.CourseMembers.Find(id);

            if (ModelState.IsValid)
            {

                _context.CourseMembers.Remove(item);
                _context.SaveChanges();
                return Json(new { success = true });


            }

            return Json(new { success = false });



        }

    }
}
