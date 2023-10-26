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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace QuanLySinhVien.Controllers.Students
{
    [Authorize(Roles = "Student")]
    public class CourseStudentsController : Controller
    {
        private readonly ElearingDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public CourseStudentsController(ElearingDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public IActionResult Index()
        {
            return View();
        }






        [Route("/AddToCourse/{alias}-{Id}", Name = "CourseStudentsAdd")]

        public IActionResult AddToCourse(string alias,int id)
        {

            //lay session vao trang admin

            string loginStudent = HttpContext.Session.GetString("AccountId_Student");

            int soNguyen = int.Parse(loginStudent);

            //lay du lieu account                   

            //truyen du lieu vao courseMember
            var InfoAccount = _context.Account.Where(x => x.Id == soNguyen).FirstOrDefault();
        

            //kiem tra sv co trong khoa hoc chua
            var IStudent = _context.CourseMembers.Where(x=> x.Course.Alias == alias && x.Mssv == InfoAccount.Code).ToList();




            //kiem tra neu id co ton tai thi se la 1
            if (IStudent.Count == 1)
            {

                return RedirectToAction("Index", "HomeStudent");



            }
            else
            {

                //them du lieu vao db ( cach thu cong )

                var AddDb = new CourseMember
                {
                    CourseId = id,
                    Attendance = 0,
                    Name = InfoAccount.FullName,
                    Mssv = InfoAccount.Code,
                    Email = InfoAccount.Email,
                    Phone = InfoAccount.Phone
                };

                _context.CourseMembers.Add(AddDb);
                _context.SaveChanges();
                return View();
            }
           
        }

        [Route("/details/{alias}-{Id}", Name = "viewDetail")]
        public async Task<IActionResult> ViewDetail_Course(int id)
        {
            var item =  _context.Courses.Where(x => x.Id == id).FirstOrDefault();

             ViewBag.IdCourse = item.Id;

            //lay session vao trang admin

            string loginAdmin = HttpContext.Session.GetString("AccountId_Student");

            ViewData["AccountId_Student"] = loginAdmin;


            //lay ra ten giang vien
            var NameGv =  _context.Account.Where(x => x.Id == item.AccountId).FirstOrDefault();

            ViewBag.NameGv = NameGv.FullName;



            //hien thi noi dung cua khoa hoc
            //partial view
           
            ViewBag.TitleCourse = item.Title;
            var ShowCourseContent = await _context.CourseContents.Where(x => x.CourseId == id).ToListAsync();   //loi load cham










            ViewBag.IsCourseContent = ShowCourseContent;


       

            

            return View(item);
        }


        [Route("/thuc-hanh/{Id}", Name = "Xemnoidungthuchanh")]

        public async Task<IActionResult> ViewExerciseContent(int id)
        {
            var items = await _context.ExerciseContents.Where(x => x.CourseContentId == id).ToListAsync();

            //lay session vao trang admin

            string loginStudent = HttpContext.Session.GetString("AccountId_Student");

            int soNguyen = int.Parse(loginStudent);
            var get_Account = await _context.Account.Where(x => x.Id == soNguyen).FirstOrDefaultAsync();

            var items_UploadAssignment = await _context.UploadAssignments.ToListAsync();   ///loi load cham

            

            ViewBag.UploadAssignment = items_UploadAssignment;

            ViewBag.GetIdAccount = get_Account.Code;

            




            return View(items);
        }

        [Route("/Member_Course/{Id}", Name = "Menber_Course")]
        public IActionResult Menber_Course(int id)
        {
            
            var items = _context.CourseMembers.Where(x => x.CourseId == id).ToList();


            var iCourse = _context.Courses.Where(x=> x.Id == id).FirstOrDefault();

            ViewBag.UrlCourse = "/details/" + iCourse.Alias + "-" + iCourse.Id;

            return View(items);
        }

        


    }
}
