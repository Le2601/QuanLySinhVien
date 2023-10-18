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

namespace QuanLySinhVien.Controllers
{
    public class HomeController : Controller
    {
        private readonly ElearingDbContext _context;

        public HomeController(ElearingDbContext context)
        {
            _context = context;
        }

        [ResponseCache(Duration = 60)] // Caching trong 60 giây
        public IActionResult Index()
        {

            //lay session vao trang admin

            string loginAdmin = HttpContext.Session.GetString("AccountId");

            ViewData["AccountId"] = loginAdmin;

            var items = _context.Department.ToList();


            //partial view 
            var ShowCourse = _context.Courses.ToList();

            ViewBag.ShowCourse = ShowCourse;




            return View(items);
        }

     

        [Route("UserLogin.html", Name = "LoginUser")]
        public IActionResult Login()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
