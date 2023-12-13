﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using QuanLySinhVien.Models;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLySinhVien.Controllers.Students
{
    [Authorize(Roles = "Student")]
    public class HomeStudentController : Controller
    {
        private readonly ElearingDbContext _context;

        public HomeStudentController(ElearingDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? page)
        {
            //lay session vao trang admin

            string loginStudent = HttpContext.Session.GetString("AccountId_Student");

            int soNguyen = int.Parse(loginStudent);

            //truyen du lieu vao courseMember
            var InfoAccount = _context.Account.Where(x => x.Id == soNguyen).FirstOrDefault();
            //tim kiem sv duoi dang object

            //lay mssv ra tim trong CourseMembers neu co khac thi hien ra khac khoa hoc ma account do chua tham gia


            //kiem tra sv co trong khoa hoc chua
            var IStudent = _context.CourseMembers.Where(x => x.Mssv != InfoAccount.Code).ToList();




            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 3;

            var items = _context.Courses.OrderBy(x=> x.Id);

            PagedList<Course> models = new PagedList<Course>(items, pageNumber, pageSize);

            ViewBag.CreateAccount = new SelectList(_context.Account.ToList(), "Id", "Title");

           
            return View(models);
        }
      
     
        public IActionResult Search(string valueCourses,int? page)

        {

            string loginStudent = HttpContext.Session.GetString("AccountId_Student");

            int soNguyen = int.Parse(loginStudent);

            //truyen du lieu vao courseMember
            var InfoAccount = _context.Account.Where(x => x.Id == soNguyen).FirstOrDefault();


            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 3;


          
                HttpContext.Session.SetString("GetValueSearch", valueCourses);
          
          

            ViewBag.GetValueSearch = HttpContext.Session.GetString("GetValueSearch");


            var items =  _context.Courses.Where(x => x.Title.Contains(valueCourses)).OrderBy(x => x.Id).ToList();

           

            //var items = await  _context.Courses.Where(x => x.Title.Contains(valueCourses)).ToListAsync();
            ViewBag.CreateAccount = new SelectList(_context.Account.ToList(), "Id", "Title");


            var CountValueSearch = _context.Courses.Where(x => x.Title.Contains(valueCourses)).OrderBy(x => x.Id).ToList();
            ViewBag.CountValueSearch = CountValueSearch.Count;

            return View(items);

        }


        public async Task<IActionResult> UserInfo(int id)
        {
            var item = await _context.Account.Where(x=> x.Id == id).FirstOrDefaultAsync();

            if(item == null)
            {
                return NotFound();
            }

            return View(item);

        }

        [Route("/chi-tiet-khoa-hoc/{Alias}-{Id}", Name = "chi-tiet-khoa-hoc")]
        public async Task<IActionResult> Detail_Course(int id)
        {

            ViewBag.department = new SelectList(_context.Department.ToList(), "Id", "Title");
            ViewBag.semester = new SelectList(_context.SemesterCourse.ToList(), "Id", "Title");
            ViewBag.Account = new SelectList(_context.Account.ToList(), "Id", "FullName") ;

            var item = await _context.Courses.Where(x => x.Id == id).FirstOrDefaultAsync();

            //diem so luong sinh vien trong khoa hoc

            var CountStudent = await _context.CourseMembers.Where(x => x.CourseId == id).CountAsync();

            ViewBag.CountStudent = CountStudent;

            return View(item);

        }




    }
}
