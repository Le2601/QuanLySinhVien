using Microsoft.AspNetCore.Authorization;
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

            var items =  _context.Courses.Where(x => x.Title.Contains(valueCourses)).OrderBy(x => x.Id);

            PagedList<Course> models =  new PagedList<Course>(items, pageNumber, pageSize);

            //var items = await  _context.Courses.Where(x => x.Title.Contains(valueCourses)).ToListAsync();
            ViewBag.CreateAccount = new SelectList(_context.Account.ToList(), "Id", "Title");

            return View(models);

        }
        

       
    }
}
