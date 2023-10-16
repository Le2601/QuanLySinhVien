using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QuanLySinhVien.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLySinhVien.Controllers.Lecturers
{
    //[Authorize] //bat xac thuc trc khi vao trang nay
    [Authorize(Roles = "Employee")]

    public class HomeLecturersController : Controller
    {
        private readonly ElearingDbContext _context;

        public HomeLecturersController(ElearingDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            return View();
        }
    }
}
