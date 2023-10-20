using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLySinhVien.Models;
using System;
using System.Linq;

namespace QuanLySinhVien.Controllers.Components
{
    public class MenuTopStudentsViewComponent : ViewComponent
    {
        private readonly ElearingDbContext _context;

        public MenuTopStudentsViewComponent(ElearingDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {

            //var taikhoan = HttpContext.Session.GetString("AccountId_Student");

            var GetNameLogin = HttpContext.Session.GetString("AccountName_Student");

            //int IdAccount_Student = Int32.Parse(taikhoan); //ep kieu string sang int


            //var item = _context.Account.Where(a => a.Id == IdAccount_Student).FirstOrDefault();

            //ViewBag.NameAccount_Student = item.FullName;

            ViewBag.GetNameLogin = GetNameLogin;


            return View();
        }

    }
}
