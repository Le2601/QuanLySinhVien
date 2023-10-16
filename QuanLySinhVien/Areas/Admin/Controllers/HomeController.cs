using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using QuanLySinhVien.Models;
using Microsoft.AspNetCore.Http;
using System;

namespace QuanLySinhVien.Areas.Admin.Controllers
{
    [Area("Admin")]

    [Authorize(Roles = "Employee,Admin")]
    public class HomeController : Controller
    {
        private readonly ElearingDbContext _context;



        public HomeController(ElearingDbContext context)
        {
            _context = context;

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
    }
}
