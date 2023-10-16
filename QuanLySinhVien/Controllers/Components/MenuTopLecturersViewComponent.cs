using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLySinhVien.Models;
using System;
using System.Linq;

namespace QuanLySinhVien.Controllers.Components
{
    public class MenuTopLecturersViewComponent : ViewComponent
    {

        private readonly ElearingDbContext _context;

        public MenuTopLecturersViewComponent(ElearingDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var taikhoan = "";
             taikhoan = HttpContext.Session.GetString("AccountId_Lecturers");

            //if (taikhoan == null)
            //{
            //    taikhoan = HttpContext.Session.GetString("AccountId_Lecturers");
            //}
           


            int IdAccount = Int32.Parse(taikhoan); //ep kieu string sang int


            var item = _context.Account.Where(a => a.Id == IdAccount).FirstOrDefault();

            ViewBag.NameAccount = item.FullName;



            return View();
        }


    }
}
