using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using QuanLySinhVien.Models;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using PagedList.Core;
using Microsoft.AspNetCore.Hosting;
using QuanLySinhVien.DI.Courses;

namespace QuanLySinhVien.Areas.Admin.Controllers
{
    [Area("Admin")]

    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly ICourseRepository _CourseRepository;
        private readonly ElearingDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public HomeController(ElearingDbContext context, ICourseRepository courseRepository, IWebHostEnvironment environment)
        {
            _CourseRepository = courseRepository;
            _context = context;

            _environment = environment;


        }


        [Route("admin.html", Name = "AdminHome")]
        public IActionResult Index()
        {
            var GetId = HttpContext.Session.GetString("AccountId");
            int IdCreate = Int32.Parse(GetId); //ep kieu string sang int

            var item = _context.Account.Where(x=> x.Id == IdCreate).FirstOrDefault();

            ViewBag.GetName = item.FullName;


            return View();
        }

        public IActionResult Search(string valueSearch, int? page)

        {

            

            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 3;

            ViewBag.department = new SelectList(_CourseRepository.GetDepartments(), "Id", "Title");
            ViewBag.semester = new SelectList(_CourseRepository.GetSemesterCourses(), "Id", "Title");


            ViewBag.AccountName = new SelectList(_CourseRepository.GetAllCreator(), "Id", "Title");

            HttpContext.Session.SetString("GetValueSearchAdmin", valueSearch);


            ViewBag.GetValueSearch = HttpContext.Session.GetString("GetValueSearchAdmin");


            var items = _context.Courses.Where(x => x.Title.Contains(valueSearch)).OrderBy(x => x.Id);

            PagedList<Course> models = new PagedList<Course>(items, pageNumber, pageSize);

          

            return View(models);

        }

    }
}
